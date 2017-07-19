namespace RemoveFiles
{
    public enum RemoveFilesOptions
    {
        /// <summary>
        /// Удаление всех файлов.
        /// </summary>
        YesToAll,
        /// <summary>
        /// Удаление текущего файла.
        /// </summary>
        Yes,
        /// <summary>
        /// Пропуск текущего файла.
        /// </summary>
        No,
        /// <summary>
        /// Пропуск всех файлов.
        /// </summary>
        NoToAll
    }
}