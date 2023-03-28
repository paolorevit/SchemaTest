using System;
using System.Collections.Generic;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace SchemaUpgradeTest
{
    [Transaction(TransactionMode.Manual)]
    public class ExternalCommand:IExternalCommand
    {
        private Document _doc;
        private Selection _selection;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                Init(commandData);
                MainCommand();
            }
            catch(Exception ex)
            {
                new TaskDialog("revit") { MainInstruction = ex.Message, MainContent = ex.StackTrace }.Show();
            }
            return Result.Succeeded;
        }
        

        private void Init(ExternalCommandData commandData)
        {
            _doc = commandData.Application.ActiveUIDocument.Document;
            _selection = commandData.Application.ActiveUIDocument.Selection;
        }
        private void MainCommand()
        {
            
            var elemToWriteStorage = SelectElement();
            if (elemToWriteStorage is null) return;

            var elemForData = SelectElement();
            if (elemForData is null) return;

            using (var trans=new Transaction(_doc))
            {
                trans.Start("Test");
                
                var dataToSave = CreateDataToSave(elemForData);
                var dataStorage = CreateDataStorage();
            
                ScatterGroupStorage.SetData(elemToWriteStorage,dataToSave);
                ScatterGroupStorage.SetData(dataStorage,dataToSave);

                trans.Commit();
            }
        }

        private DisperseDataStorage CreateDataToSave(Element elemForData)
        {
            return new DisperseDataStorage()
            {
                TopoElement = elemForData,
                ReferenceList = new List<string>(),
                AllocatedElement = elemForData,
                PickOnScreen = false,
                MonteCarlo = true,
                NumbOfElements = 10,
                Step= 10,
                VerticalStep = 10,
                Alpha = 20,
                ElementAlpha = 30,
                HeightOffset = 2,
                ElementRandomRotation = false,
                LocationLevel = elemForData,
                OscilationLevel = 5,
                ConsiderHeight = true,
                IsForEach = false
            };
        }

        private Element CreateDataStorage()
        {
            return DataStorage.Create(_doc);
        }

        private Element SelectElement()
        {
            try
            {
                var reference = _selection.PickObject(ObjectType.Element);
                return _doc.GetElement(reference);
            }
            catch
            {
                return null;
            }
        }
    }
}