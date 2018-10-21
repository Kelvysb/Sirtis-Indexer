using System.Collections.Generic;
using System.Threading.Tasks;
using SirtisIndexer.basic;

namespace SirtisIndexer.layers.backend
{
    public interface IIndexer
    {
        List<string> Extensions { get; set; }
        string Name { get; set; }
        IndexedFile ProcessFile(string p_strPath);
        Task<IndexedFile> ProcessFileAsync(string p_strPath);
        IndexedFile ApplyModifications(IndexedFile p_objFile);
        Task<IndexedFile> ApplyModificationsAsync(IndexedFile p_objFile);
    }
}