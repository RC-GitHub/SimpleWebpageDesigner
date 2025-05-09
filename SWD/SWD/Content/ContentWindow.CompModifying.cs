using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Window = System.Windows.Window;


namespace SWD.Content
{
    public partial class ContentWindow : Window
    {
        // A Function that makes sure only empty space is filled with the new size/place of a Component.
        private List<Position> GetTakenPositions(string name = "")
        {
            List<Position> takenPositions = new List<Position>();
            // If a name is specified then only the positions of components other than the one currently changed will be returned.
            if (name != "") 
            {
                foreach (Component comp in components.Values)
                {
                    if (comp.Name != currentComponent.Name) 
                        takenPositions.AddRange(comp.Positions);
                }
            }
            else
            {
                foreach (Component comp in components.Values)
                {
                    takenPositions.AddRange(comp.Positions);
                }
            }
            return takenPositions;
        }

        private void tbCompWidth_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (currentComponent == null) return;
            if (e.Key == Key.Enter)
            {
                try
                {
                    // List of taken positions
                    List<Position> takenPositions = GetTakenPositions(currentComponent.Name);
                    Debug.WriteLine(takenPositions.Count.ToString());

                    Component modifiedComponent = currentComponent;

                    int dgWidth = Int32.Parse(tbColAmount.Text);
                    int compStart = currentComponent.StartColumn;
                    int desiredWidth = Int32.Parse(tbCompWidth.Text);
                    int lastIndex = compStart + desiredWidth - 1;

                    if (desiredWidth < 0)
                    {
                        Errors.DisplayErrorMessage("The width value is incorrect!");
                    }
                    else if (lastIndex > dgWidth - 1)
                    {
                        Errors.DisplayErrorMessage("The component would go out of bounds!");
                    }
                    else
                    {
                        currentComponent.DisplayPositions();
                        modifiedComponent.Colspan = desiredWidth - 1;
                        modifiedComponent.Repopulate();
                        modifiedComponent.DisplayPositions();

                        bool modifyComponent = true;
                        foreach (Position pos in modifiedComponent.Positions)
                        {
                            if (takenPositions.Contains(pos))
                            {
                                modifyComponent = false;
                                Errors.DisplayErrorMessage("Before changing the size of a component clear out the space occupied by other ones.");
                                break;
                            }
                        }
                        if (modifyComponent)
                        {
                            Debug.WriteLine("I have come that far!");
                            components[currentComponent.Name] = modifiedComponent;
                            BuildDataGrid(true);
                        }
                    }
                }
                catch (Exception ex) { Errors.DisplayErrorMessage(ex.Message); }
            }
        }

        private void tbCompHeight_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (currentComponent == null) return;
            if (e.Key == Key.Enter)
            {
                try
                {
                    // List of taken positions
                    List<Position> takenPositions = GetTakenPositions(currentComponent.Name);
                    Debug.WriteLine(takenPositions.Count.ToString());

                    Component modifiedComponent = currentComponent;

                    int dgHeight = Int32.Parse(tbRowAmount.Text);
                    int compStart = currentComponent.StartRow;
                    int desiredHeight = Int32.Parse(tbCompHeight.Text);
                    int lastIndex = compStart + desiredHeight - 1;

                    if (desiredHeight < 0)
                    {
                        Errors.DisplayErrorMessage("The width value is incorrect!");
                    }
                    else if (lastIndex > dgHeight - 1)
                    {
                        Errors.DisplayErrorMessage("The component would go out of bounds!");
                    }
                    else
                    {
                        currentComponent.DisplayPositions();
                        modifiedComponent.Rowspan = desiredHeight - 1;
                        modifiedComponent.Repopulate();
                        modifiedComponent.DisplayPositions();

                        bool modifyComponent = true;
                        foreach (Position pos in modifiedComponent.Positions)
                        {
                            if (takenPositions.Contains(pos))
                            {
                                modifyComponent = false;
                                Errors.DisplayErrorMessage("Before changing the size of a component clear out the space occupied by other ones.");
                                break;
                            }
                        }
                        if (modifyComponent)
                        {
                            Debug.WriteLine("I have come that far!");
                            components[currentComponent.Name] = modifiedComponent;
                            BuildDataGrid(true);
                        }
                    }
                }
                catch (Exception ex) { Errors.DisplayErrorMessage(ex.Message); }
            }
        }
    }
}
