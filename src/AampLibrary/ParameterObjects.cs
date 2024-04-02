using AampLibrary.IO.Hashing;
using System.Diagnostics.CodeAnalysis;

namespace AampLibrary;

public class ParameterObjects : Dictionary<uint, ParameterObject>, IDictionary<string, ParameterObject>
{
    public ParameterObjects()
    {
    }

    public ParameterObjects(int capacity) : base(capacity)
    {
    }

    public ParameterObjects(IEnumerable<KeyValuePair<uint, ParameterObject>> collection) : base(collection)
    {
    }

    public ParameterObjects(IDictionary<uint, ParameterObject> dictionary) : base(dictionary)
    {
    }

    public ParameterObject this[string key] {
        get => base[Crc32.ComputeHash(key)];
        set => base[Crc32.ComputeHash(key)] = value;
    }

    public bool IsReadOnly => false;
    ICollection<ParameterObject> IDictionary<string, ParameterObject>.Values => Values;
    ICollection<string> IDictionary<string, ParameterObject>.Keys
        => throw new NotSupportedException(
            "Accessing string values is not supported on a hashed dictionary.");

    public void Add(string key, ParameterObject value)
    {
        Add(Crc32.ComputeHash(key), value);
    }

    public void Add(KeyValuePair<string, ParameterObject> item)
    {
        Add(Crc32.ComputeHash(item.Key), item.Value);
    }

    public bool Contains(KeyValuePair<string, ParameterObject> item)
    {
        return this[item.Key] == item.Value;
    }

    public bool ContainsKey(string key)
    {
        return ContainsKey(Crc32.ComputeHash(key));
    }

    public void CopyTo(KeyValuePair<string, ParameterObject>[] array, int arrayIndex)
    {
        foreach (var (key, value) in array.AsSpan()[arrayIndex..]) {
            this[Crc32.ComputeHash(key)] = value;
        }
    }

    public bool Remove(string key)
    {
        return Remove(Crc32.ComputeHash(key));
    }

    public bool Remove(KeyValuePair<string, ParameterObject> item)
    {
        return Remove(Crc32.ComputeHash(item.Key));
    }

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out ParameterObject value)
    {
        return TryGetValue(Crc32.ComputeHash(key), out value);
    }

    IEnumerator<KeyValuePair<string, ParameterObject>> IEnumerable<KeyValuePair<string, ParameterObject>>.GetEnumerator()
        => throw new NotSupportedException(
            "Accessing string values is not supported on a hashed dictionary.");
}
