using System.IO;
using System.Text;

namespace CsvHandler
{
    /// <summary>
    /// Interface for CSV file handling operations.
    /// </summary>
    public interface ICsvHandler
    {
        /// <summary>
        /// Gets or sets the file name for the CSV file.
        /// </summary>
        string File_Name { get; set; }

        /// <summary>
        /// Gets or sets the encoding used for reading and writing the CSV file.
        /// </summary>
        Encoding File_Encoding { get; set; }

        /// <summary>
        /// Gets or sets the delimiter used to separate values in the CSV file.
        /// </summary>
        string Csv_Delimiter { get; set; }

        /// <summary>
        /// Gets or sets the flag for whether the file has a header line or not
        /// </summary>
        bool Has_Header { get; set; }

        /// <summary>
        /// Gets or sets the CSV header as a string list
        /// </summary>
        List<string> Header { get; set; }

        /// <summary>
        /// Reads the CSV file and returns a list of rows, where each row is represented as a list of strings.
        /// </summary>
        /// <param name="skip_header">Indicates whether the first row is a header and should be skipped.</param>
        /// <returns>A list of rows, where each row is represented as a list of strings.</returns>
        /// <exception cref="HandlerExceptions.InvalidCSVException"></exception>
        /// <exception cref="HandlerExceptions.InvalidArgumentException"></exception>
        List<List<string>> Read(bool skip_header = false);

        /// <summary>
        /// Reads the CSV file and returns its contents as a dictionary, where the keys are the header columns and the values are the corresponding row values.
        /// </summary>
        /// <param name="skip_header">Indicates whether the first row is a header and should be skipped.</param>
        /// <returns>A list of dictionaries, where each dictionary represents a row with header columns as keys.</returns>
        /// <exception cref="HandlerExceptions.InvalidArgumentException"></exception>
        /// <exception cref="HandlerExceptions.InvalidCSVException"></exception>
        List<Dictionary<string, string>> ReadAsDictionaryList();

        /// <summary>
        /// Writes the provided data rows to the CSV file, either overwriting or appending based on the specified file mode.
        /// </summary>
        /// <param name="data_rows">The list of rows to write to the CSV file.</param>
        /// <param name="new_line">The newline character to use.</param>
        /// <param name="file_mode">The write mode, either overwrite or append.</param>
        /// <exception cref="HandlerExceptions">Thrown when an error occurs while writing to the CSV file.</exception>
        void Write(List<List<string>> data_rows, string? new_line = null, WriteMode? write_mode = null, bool write_header = true);
    }

    public class CsvHandler : ICsvHandler
    {
        #region Constants

        const string DEFAULT_ENCODING = "UTF-8";
        const string DEFAULT_DELIMITER = ",";
        const string KEY_ROW_INDEX = "row_index";

        #endregion

        #region Constructor

        public CsvHandler(
            string file_name,
            Encoding? encoding = null,
            Delimiter? delimiter = null,
            bool has_header = true,
            List<string>? header = null
            )
        {
            File_Name = file_name;
            File_Encoding = encoding ?? Encoding.GetEncoding(DEFAULT_ENCODING);
            Csv_Delimiter = delimiter?.Value ?? DEFAULT_DELIMITER;
            Has_Header = has_header;
            Header = header ?? new List<string>();
        }

        #endregion

        #region Class members

        public string File_Name { get; set; }
        public Encoding File_Encoding { get; set; }
        public string Csv_Delimiter { get; set; }
        public bool Has_Header { get; set; }
        public List<string> Header { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Reads the CSV file and returns a list of rows, where each row is represented as a list of strings.
        /// </summary>
        /// <returns>A list of rows, where each row is represented as a list of strings.</returns>
        /// <param name="skip_header">Indicates whether the first row is a header and should be skipped.</param>
        /// <exception cref="HandlerExceptions.InvalidCSVException"></exception>
        public List<List<string>> Read(bool skip_header = false)
        {
            List<List<string>> data_rows = new List<List<string>>();
            int row_index = 0;
            StreamReader? reader = null;

            try
            {
                reader = new StreamReader(new FileStream(File_Name, FileMode.Open, FileAccess.Read), File_Encoding);
                string? line;

                while ((line = reader.ReadLine()) != null)
                {
                    if (row_index == 0)
                    {
                        SetHeader(line);

                        if (skip_header && Has_Header)
                        {
                            // Skip the header row if specified
                            row_index++;
                            continue;
                        }
                    }

                    data_rows.Add(InspectLine(line, ref reader, ref row_index));
                    row_index++;
                }
                return data_rows;
            }
            catch (HandlerExceptions.InvalidCSVException ex)
            {
                throw new HandlerExceptions.InvalidCSVException($"Error reading CSV file at row {row_index}: {ex.Message}");
            }
            finally
            {
                reader?.Dispose();
            }
        }

        /// <summary>
        /// Reads the CSV file and returns its contents as a dictionary, where the keys are the header columns and the values are the corresponding row values.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HandlerExceptions.InvalidArgumentException"></exception>
        /// <exception cref="HandlerExceptions.InvalidCSVException"></exception>
        public List<Dictionary<string, string>> ReadAsDictionaryList()
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            List<List<string>> data_rows = Read(true);
            int header_count = Header.Count;

            if (header_count == 0)
            {
                throw new HandlerExceptions.InvalidArgumentException("Header is empty. Cannot read CSV as dictionary.");
            }

            bool row_index_useable = !Header.Contains(KEY_ROW_INDEX);   // Check if the row index key is not already present in the header

            for (int i = 0; i < data_rows.Count; i++)
            {
                List<string> row = data_rows[i];
                if (row.Count != header_count)
                {
                    throw new HandlerExceptions.InvalidCSVException($"Row has a different number of columns ({row.Count}) than the header ({header_count}).");
                }

                Dictionary<string, string> row_dict = new Dictionary<string, string>();

                if (row_index_useable)
                    row_dict[KEY_ROW_INDEX] = (i + 1).ToString(); // Add the row index to the dictionary if usable

                for (int j = 0; j < header_count; j++)
                {
                    row_dict[Header[j]] = row[j];
                }
                result.Add(row_dict);
            }

            return result;
        }

        /// <summary>
        /// Writes the provided data rows to the CSV file, either overwriting or appending based on the specified file mode.
        /// </summary>
        /// <param name="data_rows">The list of rows to write to the CSV file.</param>
        /// <param name="new_line">The newline character to use.</param>
        /// <param name="file_mode">The write mode, either overwrite or append.</param>
        /// <exception cref="HandlerExceptions">Thrown when an error occurs while writing to the CSV file.</exception>
        public void Write(List<List<string>> data_rows, string? new_line = null, WriteMode? write_mode = null, bool write_header = true)
        {
            StreamWriter? writer = null;

            try
            {
                if (write_mode is null || write_mode == WriteMode.Overwrite)
                {
                    if (File.Exists(File_Name)) // Delete the file if it exists to overwrite it
                        File.Delete(File_Name);

                    writer = new StreamWriter(new FileStream(File_Name, FileMode.Create, FileAccess.Write), File_Encoding);
                }
                else if (write_mode == WriteMode.Append)
                {
                    writer = new StreamWriter(new FileStream(File_Name, FileMode.Append, FileAccess.Write), File_Encoding);
                }
                else
                {
                    throw new HandlerExceptions.InvalidArgumentException("Invalid write mode");
                }

                // Set the newline character if provided
                string newline_character = new_line ?? Environment.NewLine;
                writer.NewLine = newline_character;

                if (write_header)
                {
                    if (Header.Count == 0)
                    {
                        throw new HandlerExceptions.InvalidArgumentException("Header is empty. Cannot write header to CSV file.");
                    }
                    else
                    {
                        writer.WriteLine(JoinRow(Header));
                    }
                }

                foreach (List<string> row in data_rows)
                {
                    // Join the row elements with the specified delimiter and write to the file
                    writer.WriteLine(JoinRow(row));
                }
            }
            finally
            {
                writer?.Close();
            }
        }

        #endregion

        #region Private Helper Methods

        private List<string>? SetHeader(string row)
        {
            if (!Has_Header) return null;
            else if (string.IsNullOrWhiteSpace(row))
                throw new HandlerExceptions.InvalidArgumentException("Header row cannot be null or empty.");

            Header = SplitLine(row);

            if (IsDuplicated(Header))
            {
                HashSet<string> seen = new HashSet<string>();
                foreach (string header in Header)
                {
                    if (!seen.Add(header))
                    {
                        throw new HandlerExceptions.InvalidArgumentException("Duplicate header found: " + header);
                    }
                }
            }

            return Header;
        }

        private List<string> InspectLine(string line, ref StreamReader reader, ref int row_index)
        {
            List<string> split_line_result = SplitLine(line);

            while (RowContainsNewline(line, ref split_line_result))
            {
                string? next_line = reader.ReadLine() ?? throw new HandlerExceptions.InvalidCSVException("Unexpected end of file while reading a multiline row.");
                line += Environment.NewLine + next_line;
                row_index++;
            }

            return split_line_result;
        }

        /// <summary>
        /// Checks if the last element of the split line contains a newline character, indicating that the row is not complete.
        /// </summary>
        /// <param name="current_row"></param>
        /// <param name="split_line_result"></param>
        /// <returns>True if the last element contains a newline character, false otherwise.</returns>
        private bool RowContainsNewline(string current_row, ref List<string> split_line_result)
        {
            split_line_result = SplitLine(current_row);

            if (split_line_result.Count == 0)
            {
                return false;
            }

            string last_element = split_line_result[split_line_result.Count - 1];
            if (last_element.EndsWith('\n') || last_element.EndsWith('\r'))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Splits a line of text into a list of strings based on the specified delimiter.
        /// </summary>
        /// <param name="line">The line of text to split.</param>
        /// <returns>A list of strings representing the split line.</returns>
        /// <exception cref="HandlerExceptions.InvalidCSVException">Thrown when the line cannot be split correctly.</exception>
        private List<string> SplitLine(string line)
        {
            List<string> result_list = new List<string>();
            StringBuilder current_value = new StringBuilder();
            bool inside_quotes = false;
            int index = 0;

            foreach (char c in line)
            {
                if (c == '"')
                {
                    if (!inside_quotes && index > 0 && line[index - 1] == '"')
                    {
                        // Escaped quote, add it to the current value
                        current_value.Append(c);
                    }

                    // Toggle the inside_quotes flag when encountering a quote character
                    inside_quotes = !inside_quotes;
                }
                else if (c.ToString() == Csv_Delimiter && !inside_quotes)
                {
                    // Delimiter found, add the current value to the result list and reset current_value
                    result_list.Add(current_value.ToString());
                    current_value.Clear();
                }
                else if (c == '\n' || c == '\r')
                {
                    // Newline character
                    if (inside_quotes)
                        current_value.Append(c); // Inside quotes, treat as part of the value
                    else
                        break; // Outside quotes, treat as end of line
                }
                else
                {
                    // Regular character, add it to the current value
                    current_value.Append(c);
                }
                index++;
            }

            if (inside_quotes)
                current_value.Append(Environment.NewLine); // If still inside quotes, add a newline to the current value

            // Add the last value to the result list
            result_list.Add(current_value.ToString());
            return result_list;
        }

        private bool IsDuplicated(List<string> row_elements)
        {
            HashSet<string> seen_items = [];
            return row_elements.Any(x => !seen_items.Add(x));
        }

        /// <summary>
        /// Joins a list of strings into a single string using the specified delimiter, handling quotes and escaping as necessary.
        /// </summary>
        /// <param name="row">The list of strings representing a row in the CSV file.</param>
        /// <returns>A single string representing the joined row.</returns>
        private string JoinRow(List<string> row)
        {
            StringBuilder joined_row = new StringBuilder();
            for (int i = 0; i < row.Count; i++)
            {
                string value = row[i];
                // If the value contains the delimiter, newline, or an odd number of quotes, wrap it in quotes and escape any existing quotes
                if (value.Contains(Csv_Delimiter) || value.Contains('\n') || value.Contains('\r') || value.Count(c => c == '"') % 2 != 0)
                {
                    // Escape quotes in the value
                    if (value.Contains('"'))
                    {
                        value = value.Replace("\"", "\"\"");
                    }
                    value = $"\"{value}\"";
                }
                joined_row.Append(value);
                if (i < row.Count - 1)
                {
                    joined_row.Append(Csv_Delimiter);
                }
            }
            return joined_row.ToString();
        }

        #endregion
    }

    public class Delimiter
    {
        private Delimiter(string value) { Value = value; }
        public string Value { get; private set; }

        public static Delimiter Comma { get { return new Delimiter(","); } }
        public static Delimiter Tab { get { return new Delimiter("\t"); } }
        public static Delimiter Semicolon { get { return new Delimiter(";"); } }
        public static Delimiter Pipe { get { return new Delimiter("|"); } }
    }

    public class WriteMode
    {
        private WriteMode(string value) { Value = value; }
        public string Value { get; private set; }

        public static WriteMode Overwrite { get { return new WriteMode("w"); } }
        public static WriteMode Append { get { return new WriteMode("a"); } }
    }

    public class HandlerExceptions : Exception
    {
        public HandlerExceptions(string message) : base(message) { }
        public HandlerExceptions(string message, Exception inner_exception) : base(message, inner_exception) { }

        public class InvalidCSVException : HandlerExceptions
        {
            public InvalidCSVException(string message = "") : base($"Invalid CSV format: {message}") { }
        }

        public class InvalidArgumentException : HandlerExceptions
        {
            public InvalidArgumentException(string message = "") : base($"Invalid argument: {message}") { }
        }
    }
}