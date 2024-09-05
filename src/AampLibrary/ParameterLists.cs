using System.Diagnostics.CodeAnalysis;
using AampLibrary.IO;
using AampLibrary.IO.Hashing;

namespace AampLibrary;

public class ParameterLists : SortedDictionary<uint, ParameterList>, IDictionary<string, ParameterList>
{
    public ParameterLists() : base(BinaryUInt32Comparer.Instance)
    {
    }

    public ParameterLists(IEnumerable<KeyValuePair<uint, ParameterList>> collection) : base(collection.ToDictionary(kvp => kvp.Key, kvp => kvp.Value), BinaryUInt32Comparer.Instance)
    {
    }

    public ParameterLists(IDictionary<uint, ParameterList> dictionary) : base(dictionary, BinaryUInt32Comparer.Instance)
    {
    }

    public ParameterList this[string key] {
        get => base[Crc32.ComputeHash(key)];
        set => base[Crc32.ComputeHash(key)] = value;
    }

    public bool IsReadOnly => false;
    ICollection<ParameterList> IDictionary<string, ParameterList>.Values => Values;
    ICollection<string> IDictionary<string, ParameterList>.Keys
        => throw new NotSupportedException(
            "Accessing string values is not supported on a hashed dictionary.");

    public void Add(string key, ParameterList value)
    {
        Add(Crc32.ComputeHash(key), value);
    }

    public void Add(KeyValuePair<string, ParameterList> item)
    {
        Add(Crc32.ComputeHash(item.Key), item.Value);
    }

    public bool Contains(KeyValuePair<string, ParameterList> item)
    {
        return this[item.Key] == item.Value;
    }

    public bool ContainsKey(string key)
    {
        return ContainsKey(Crc32.ComputeHash(key));
    }

    public void CopyTo(KeyValuePair<string, ParameterList>[] array, int arrayIndex)
    {
        foreach ((string key, ParameterList value) in array.AsSpan()[arrayIndex..]) {
            this[Crc32.ComputeHash(key)] = value;
        }
    }

    public bool Remove(string key)
    {
        return Remove(Crc32.ComputeHash(key));
    }

    public bool Remove(KeyValuePair<string, ParameterList> item)
    {
        return Remove(Crc32.ComputeHash(item.Key));
    }

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out ParameterList value)
    {
        return TryGetValue(Crc32.ComputeHash(key), out value);
    }

    IEnumerator<KeyValuePair<string, ParameterList>> IEnumerable<KeyValuePair<string, ParameterList>>.GetEnumerator()
        => throw new NotSupportedException(
            "Accessing string values is not supported on a hashed dictionary.");
}
