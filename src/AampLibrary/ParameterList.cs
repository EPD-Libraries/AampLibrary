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

    internal ParameterList(ref ImmutableAamp aamp, ref AampParameterList parameterList, IAampNameResolver aampNameResolver)
    {
        Lists = new(parameterList.ListCount);
        for (int i = 0; i < parameterList.ListCount; i++) {
            ref AampParameterList pList = ref aamp.GetList(i, parameterList.ListsOffset);
            Lists[pList.Name]
                = new(ref aamp, ref pList, aampNameResolver);
        }

        Objects = new(parameterList.ObjectCount);
        for (int i = 0; i < parameterList.ObjectCount; i++) {
            ref AampParameterObject pObj = ref aamp.GetObject(i, parameterList.ObjectsOffset);
            Objects[pObj.Name]
                = new(ref aamp, ref pObj, aampNameResolver);
        }
    }
}
