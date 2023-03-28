using Autodesk.Revit.DB;

namespace SchemaUpgradeTest
{
    public class AdvancedScatterElement
    {
        public AdvancedScatterElement(ElementId familySymbolId, double size, int ratio)
        {
            FamilySymbolId = familySymbolId;
            Size = size;
            Ratio = ratio;
        }

        public ElementId FamilySymbolId { get; private set; }
        public double Size { get; private set; }
        public int Ratio { get; private set; }
    }
}