using FizzWare.NBuilder.Implementation;

namespace FizzWare.NBuilder.Tests.Unit.Extensibility
{
    public class EvenDeclaration<T> : Declaration<T>
    {
        public EvenDeclaration(IListBuilderImpl<T> listBuilderImpl, IObjectBuilder<T> objectBuilder)
            : base(listBuilderImpl, objectBuilder)
        {
        }

        public override int NumberOfAffectedItems => listBuilderImpl.Capacity / 2;

        public override int Start => 0;

        public override int End => listBuilderImpl.Capacity - 1;

        public override void Construct()
        {
            for (var i = 0; i < NumberOfAffectedItems; i++)
                myList.Add(objectBuilder.Construct(i));
        }

        public override void AddToMaster(T[] masterList)
        {
            for (int i = 0, j = 0; i < myList.Count; i++, j += 2)
            {
                MasterListAffectedIndexes.Add(j);
                masterList[j] = myList[i];
            }
        }
    }
}