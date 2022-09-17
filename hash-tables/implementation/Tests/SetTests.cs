using Microsoft.VisualStudio.TestTools.UnitTesting;

using HashTables;

[TestClass]
public class SetTests : HashTableTests<string>
{
    public SetTests()
        : base(new Set<string>(), GetKeys(Data.SetsDistinct))
    { }
}