using AampLibrary.Structures;

namespace AampLibrary;

public class ParameterList
{
    public ParameterObjects Objects { get; set; }
    public ParameterLists Lists { get; set; }

    public ParameterList()
    {
        Objects = [];
        Lists = [];
    }

    internal ParameterList(ref ImmutableAamp aamp, ref AampParameterList parameterList, int offset)
    {
        Lists = new(parameterList.ListCount);
        for (int i = 0; i < parameterList.ListCount; i++) {
            ref AampParameterList pList = ref aamp.GetList(i, offset + parameterList.ListsOffset * 4, out int listOffset);
            Lists[pList.Name] = new(ref aamp, ref pList, listOffset);
        }

        Objects = new(parameterList.ObjectCount);
        for (int i = 0; i < parameterList.ObjectCount; i++) {
            ref AampParameterObject pObj = ref aamp.GetObject(i, offset + parameterList.ObjectsOffset * 4, out int objectOffset);
            Objects[pObj.Name] = new ParameterObject(ref aamp, ref pObj, objectOffset);
        }
    }
}
