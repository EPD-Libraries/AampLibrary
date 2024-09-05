using AampLibrary.Structures;

namespace AampLibrary;

public class ParameterList
{
    public ParameterObjects Objects { get; init; } = [];
    public ParameterLists Lists { get; init; } = [];

    public ParameterList()
    {
    }

    internal ParameterList(ref ImmutableAamp aamp, ref AampParameterList parameterList, int offset)
    {
        for (int i = 0; i < parameterList.ListCount; i++) {
            ref AampParameterList pList = ref aamp.GetList(i, offset + parameterList.ListsOffset * 4, out int listOffset);
            Lists[pList.Name] = new(ref aamp, ref pList, listOffset);
        }

        for (int i = 0; i < parameterList.ObjectCount; i++) {
            ref AampParameterObject pObj = ref aamp.GetObject(i, offset + parameterList.ObjectsOffset * 4, out int objectOffset);
            Objects[pObj.Name] = new ParameterObject(ref aamp, ref pObj, objectOffset);
        }
    }
}
