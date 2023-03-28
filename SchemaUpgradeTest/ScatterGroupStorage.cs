using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;

namespace SchemaUpgradeTest
{
    public class ScatterGroupStorage
    {
        private static Guid _guid = new Guid("4D68F6F9-AC21-4E8A-BA66-99A6622AE8CD");
        private static Schema GetOrCreateSchema()
        {
            var schema = Schema.Lookup(_guid);
            if (IsSchemaValid(schema)) return schema;
            
            return CreateSchema();
        }

        private static bool IsSchemaValid(Schema schema)
        {
            return schema?.IsValidObject ?? false;
        }
        private static Schema CreateSchema()
        {
            SchemaBuilder schemaBuilder = new SchemaBuilder(_guid);
            schemaBuilder.SetReadAccessLevel(AccessLevel.Public);
            schemaBuilder.SetWriteAccessLevel(AccessLevel.Public);
            schemaBuilder.SetSchemaName(nameof(ScatterGroupStorage));
            schemaBuilder.AddSimpleField(nameof(Field.HostElemId), typeof(ElementId));
            schemaBuilder.AddArrayField(nameof(Field.ReferenceList), typeof(string));
            schemaBuilder.AddSimpleField(nameof(Field.ScatterElemId), typeof(ElementId));
            schemaBuilder.AddSimpleField(nameof(Field.PickOnScreenStatus), typeof(bool));
            schemaBuilder.AddSimpleField(nameof(Field.IsIrregular), typeof(bool));
            schemaBuilder.AddSimpleField(nameof(Field.NumberOfElements), typeof(int));
            schemaBuilder.AddSimpleField(nameof(Field.LengthStep), typeof(double)).SetSpec(SpecTypeId.Length);
            schemaBuilder.AddSimpleField(nameof(Field.WidthStep), typeof(double)).SetSpec(SpecTypeId.Length);
            schemaBuilder.AddSimpleField(nameof(Field.PatternAngle), typeof(double)).SetSpec(SpecTypeId.Angle);
            schemaBuilder.AddSimpleField(nameof(Field.ElementAngle), typeof(double)).SetSpec(SpecTypeId.Angle);
            schemaBuilder.AddSimpleField(nameof(Field.OffsetFromLevel), typeof(double)).SetSpec(SpecTypeId.Length);
            schemaBuilder.AddSimpleField(nameof(Field.RandomizingValue), typeof(int));
            schemaBuilder.AddSimpleField(nameof(Field.LevelId), typeof(ElementId));
            schemaBuilder.AddSimpleField(nameof(Field.IsSurfaceElevation), typeof(bool));
            schemaBuilder.AddSimpleField(nameof(Field.IsElementAngleRandom), typeof(bool));
            schemaBuilder.AddSimpleField(nameof(Field.IsForEach), typeof(bool));
            return schemaBuilder.Finish();
        }
        
        public static void SetData(Element elem, DisperseDataStorage data)
        {
            var schema = GetOrCreateSchema();
            Entity entity = new Entity(schema);

            entity.Set(nameof(Field.HostElemId), data.TopoElement.Id);
            entity.Set(nameof(Field.ReferenceList), data.ReferenceList);
            entity.Set(nameof(Field.ScatterElemId), data.AllocatedElement is null||!data.AllocatedElement.IsValidObject?ElementId.InvalidElementId: data.AllocatedElement.Id);
            entity.Set(nameof(Field.PickOnScreenStatus), data.PickOnScreen);
            entity.Set(nameof(Field.IsIrregular), data.MonteCarlo);
            entity.Set(nameof(Field.NumberOfElements), data.GroupIds.Count);
            entity.Set(nameof(Field.LengthStep), data.Step, UnitTypeId.Feet);
            entity.Set(nameof(Field.WidthStep), data.VerticalStep, UnitTypeId.Feet);
            entity.Set(nameof(Field.PatternAngle), data.Alpha, UnitTypeId.Degrees);
            entity.Set(nameof(Field.ElementAngle), data.ElementAlpha, UnitTypeId.Degrees);
            entity.Set(nameof(Field.OffsetFromLevel), data.HeightOffset, UnitTypeId.Feet);
            entity.Set(nameof(Field.IsElementAngleRandom), data.ElementRandomRotation);
            entity.Set(nameof(Field.LevelId), data.ConsiderHeight ? ElementId.InvalidElementId : data.LocationLevel.Id);
            entity.Set(nameof(Field.RandomizingValue), data.OscilationLevel);
            entity.Set(nameof(Field.IsSurfaceElevation), data.ConsiderHeight);
            entity.Set(nameof(Field.IsForEach), data.IsForEach);

            elem.SetEntity(entity);
        }


        private enum Field
        {
            HostElemId,
            ReferenceList,
            ScatterElemId,
            PickOnScreenStatus,
            IsIrregular,
            NumberOfElements,
            LengthStep,
            WidthStep,
            RandomizingValue,
            PatternAngle,
            LevelId,
            IsSurfaceElevation,
            OffsetFromLevel,
            ElementAngle,
            IsElementAngleRandom,
            IsForEach
        }
    }
}