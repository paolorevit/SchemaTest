using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace SchemaUpgradeTest
{
    public class DisperseDataStorage
    {
        public bool PickOnScreen { get; set; } = false; 

        public double Step { get; set; } =300; 

        public double VerticalStep { get; set; } = 0; 
        public int NumbOfElements { get; set; } = -1; 
        public double Depth { get; set; } = 1000000; 
        public bool ConsiderHeight { get; set; } = false; 
        public double HeightOffset { get; set; } = 0; 
        public bool CreateAssembly { get; set; } = false; 
        public bool CreateGroupOnlyElements { get; set; } = true; 
        public bool RotationByLine { get; set; } = false;
        public bool MonteCarlo { get; set; } = false; 
        public double Alpha { get; set; } = 0; 
        public double ElementAlpha { get; set; } = 0;
        public bool ElementRandomRotation { get; set; } = false; 
        public int OscilationLevel { get; set; } = 0; 
        public Element LocationLevel { get; set; }
        public double LevelElevation { get; set; } = 0;

        public ICollection<ElementId> GroupIds { get; set; } = new List<ElementId>();

        public Element TopoElement { get; set; } =null; 

        public Element AllocatedElement { get; set; } = null; 
        public double UnitMultiplier { get; set; } = 1; 

        public bool TestMode { get; set; } = false; 
        public int PointsLimiter { get; set; } = 1000; 
        public bool IsForEach { get; set; }
        public Element RelatedGroup { get; set; }
        public IList<string> ReferenceList { get; set; }
        public bool IsAdvancedScatter { get; set; }
        public List<AdvancedScatterElement> AdvancedScatterList { get; set; }
        public bool IsWidthAndLengthSame { get; set; }
        public bool IsFindMyHost { get; set; }
        public bool TriangleGrid { get; set; }

        public static DisperseDataStorage Default() => new DisperseDataStorage(){Step = 0,NumbOfElements = 0};
    }
}