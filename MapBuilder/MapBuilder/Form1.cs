namespace MapBuilder
{
    public partial class Form1 : Form
    {
        private string mapPath1 = string.Empty;
        private string mapPath2 = string.Empty;
        public Form1()
        {
            InitializeComponent();
        }

        private Boolean AreTextBoxesNotNull()
        {
            if (String.IsNullOrEmpty(newMapName.Text) || String.IsNullOrEmpty(mapPath1) || String.IsNullOrEmpty(mapPath2))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private static Boolean ValidFileName(string filename)
        {
            foreach (char c in filename.ToCharArray())
            {
                if(Path.GetInvalidFileNameChars().Contains(c))
                {
                    return false;
                }
            }
            return true;
        }

        private static void CheckSeams(string filepath, StreamWriter writer, Boolean verticalSeam)
        {
            StreamReader sr = new(filepath);
            string[] mapLines = sr.ReadToEnd()!.Split('\n');
            //naive solution: change every edge line on the seam to have 1's on the sides touching the seam
            //regardless of if the tiles have an elevation difference
            //possible issue: LoS calculations are not understood and if they use the edge lines
            //having all the seams set to 1 can cause unintended issues with LoS

            //dedicated solution will be very hard to implement and will require comparing 
            //each of the 2-3 hexes on the opposite side of the seam for elevation differences

            string[] firstLine = mapLines[0].Split(';');
            //magic number that means we can use 1 dynamic block of code instead of 2 near identical ones
            int i = verticalSeam ? 1 : 0;
            //map dimensions are saved as 2 bigger than they really are, divide by 2 to get where the seam is
            int separator = (Convert.ToInt32(firstLine[3 + i]) - 2) / 2;
            foreach(string line in mapLines)
            {
                if (line[0] == 'E')
                {

                    /**
                     * E lines have the form E;y;x;a;b;c;d;e;f
                     * where a-f are either 0 or 1 and refer to one side of the hex, starting from the top left:
                     *     
                     *     b
                     * a  /‾‾\ c
                     * f  \__/ d
                     *     e
                     *     
                     * thus for vertical seams only c & d (on left of seam) and a & f (on right) need to be changed
                     * and for horizontal seams f, e, & d (on top of seam) and a, b, & c (on bottom)
                     * an additional consideration for horizontal seams is that, due to the geometry of hexagons,
                     * every second hex may only need either b or e changed: /‾‾\__/‾‾\__/
                     * this depends on if naively changed f, e, & d uniformly affects LoS
                     */

                    string[] elements = line.Split(';');
                    //if this E line refers to a tile on the left or top of the seam
                    if (Convert.ToInt32(elements[1 + i]) == separator - 1)
                    {
                        //c & d
                        if (verticalSeam)
                        {
                            elements[5] = "1";
                            elements[6] = "1";
                            writer.WriteLine(string.Join(";", elements));
                        }
                        //f, e, & d
                        else
                        {
                            elements[6] = "1";
                            elements[7] = "1";
                            elements[8] = "1";
                            writer.WriteLine(string.Join(";", elements));
                        }
                    }
                    //if this E line refers to a tile on the right or bottom of the seam
                    if (Convert.ToInt32(elements[1 + i]) == separator)
                    {
                        //a & f
                        if (verticalSeam)
                        {
                            elements[3] = "1";
                            elements[8] = "1";
                            writer.WriteLine(string.Join(";", elements));
                        }
                        //a, b, & c
                        else
                        {
                            elements[3] = "1";
                            elements[4] = "1";
                            elements[5] = "1";
                            writer.WriteLine(string.Join(";", elements));
                        }
                    }

                }
                else
                {
                    writer.WriteLine(line);
                }
            }

            sr.Dispose();
        }

        private void MapLoad1_Click(object sender, EventArgs e)
        {
            string testPath = @"C:\Users\Arthur\source\repos\ConsoleApp1\ConsoleApp1\map builder\";

            using (OpenFileDialog openFileDialog = new())
            {
                openFileDialog.InitialDirectory = testPath;
                openFileDialog.Filter = "Map files |*.map|All files |*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    string filePath = openFileDialog.FileName;
                    mapPath1 = filePath;
                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using StreamReader reader = new(fileStream);
                    string fileContent = reader.ReadToEnd();
                }
            }
            mapName1.Text = "Selected: " + Path.GetFileName(mapPath1);
        }
        private void MapLoad2_Click(object sender, EventArgs e)
        {
            string testPath = @"C:\Users\Arthur\source\repos\ConsoleApp1\ConsoleApp1\map builder\";

            using (OpenFileDialog openFileDialog = new())
            {
                openFileDialog.InitialDirectory = testPath;
                openFileDialog.Filter = "Map files |*.map|All files |*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    string filePath = openFileDialog.FileName;
                    mapPath2 = filePath;
                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using StreamReader reader = new(fileStream);
                    string fileContent = reader.ReadToEnd();
                }
            }
            mapName2.Text = "Selected: " + Path.GetFileName(mapPath2);
        }

        private void AddToLeft(object sender, EventArgs e)
        {
            if (AreTextBoxesNotNull())
            {
                if (ValidFileName(newMapName.Text))
                {
                    StreamReader leftReader = new(mapPath2);
                    StreamReader rightReader = new(mapPath1);

                    //sets the target directory for the writer to the 'map builder' folder in the repo
                    //TODO: change so that, when ran from the SF folder, the target is the custom maps folder
                    string targetPath = Path.Combine(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName, "map builder"), newMapName.Text + ".map");
                    StreamWriter streamWriter = new(targetPath);
                    string[] leftFirstLine = leftReader.ReadLine()!.Split(';');
                    //first line of every map file has the form
                    //Map;mapName;number of maps made in session(irrelevant);height;width;unknownx7;series of dozens of semicolons for unknown reason
                    int leftWidth = Convert.ToInt32(leftFirstLine[4]);
                    int rightWidth = Convert.ToInt32(rightReader.ReadLine()!.Split(";")[4]);

                    leftFirstLine[1] = newMapName.Text;
                    leftFirstLine[4] = Convert.ToString(leftWidth + rightWidth - 2);

                    streamWriter.WriteLine(string.Join(';', leftFirstLine));

                    //the entirety of the left map is copied over except for the rightmost two columns which are truncated by the right map
                    while (leftReader.Peek() >= 0)
                    {
                        string line = leftReader.ReadLine()!;
                        string[] elements = line.Split(';');
                        if (Convert.ToInt32(elements[2]) < leftWidth - 2)
                        {
                            streamWriter.WriteLine(line);
                        }
                    }

                    while (rightReader.Peek() >= 0)
                    {
                        string line = rightReader.ReadLine()!;
                        string[] elements = line.Split(";");
                        if (Convert.ToInt32(elements[2]) == 0 && elements[0] == "T")
                        {
                            //convert the 0th tile of each row of the right map from:
                            //T;y;x;4;4;4;4;8;0 (for even rows)
                            //T;y;x;4;0;4;0;8;0 (for odd rows)
                            //into
                            //T;y;x;0;0;0;0;8;0 the '8' currently has an unknown effect 
                            //but usually non-boundary tiles will have a '0' instead
                            //leaving the '8' does not affect map loading
                            for (int i = 3; i < 7; i++)
                            {
                                elements[i] = "0";
                            }
                            elements[2] = Convert.ToString(Convert.ToInt32(elements[2]) + leftWidth - 2);
                            streamWriter.WriteLine(string.Join(";", elements));
                        }
                        else
                        {
                            elements[2] = Convert.ToString(Convert.ToInt32(elements[2]) + leftWidth - 2);
                            streamWriter.WriteLine(string.Join(";", elements));
                        }
                    }
                    //TODO: handle logic for when new edges have different elevations
                    CheckSeams(targetPath, streamWriter, true);

                    leftReader.Dispose();
                    rightReader.Dispose();
                    streamWriter.Dispose();
                    MessageBox.Show("Map succesfully created.", "Success", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("Invalid new map name. You cannot use these characters when naming a new map:\n" + new String(Path.GetInvalidFileNameChars()), "Error", MessageBoxButtons.OK);
                }
            }
            else
            {
                MessageBox.Show("Check to make sure you have selected two maps and have entered a new name in the text box.", "Error", MessageBoxButtons.OK);
            }
        }

        private void AddToRight(object sender, EventArgs e)
        {
            if (AreTextBoxesNotNull())
            {
                if (ValidFileName(newMapName.Text))
                {
                    StreamReader leftReader = new(mapPath1);
                    StreamReader rightReader = new(mapPath2);

                    //sets the target directory for the writer to the 'map builder' folder in the repo
                    //TODO: change so that, when ran from the SF folder, the target is the custom maps folder
                    string targetPath = Path.Combine(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName, "map builder"), newMapName.Text + ".map");
                    StreamWriter streamWriter = new(targetPath);
                    string[] leftFirstLine = leftReader.ReadLine()!.Split(';');
                    //first line of every map file has the form
                    //Map;mapName;number of maps made in session(irrelevant);height;width;unknownx7;series of dozens of semicolons for unknown reason
                    int leftWidth = Convert.ToInt32(leftFirstLine[4]);
                    int rightWidth = Convert.ToInt32(rightReader.ReadLine()!.Split(";")[4]);

                    leftFirstLine[1] = newMapName.Text;
                    leftFirstLine[4] = Convert.ToString(leftWidth + rightWidth - 2);

                    streamWriter.WriteLine(string.Join(';', leftFirstLine));

                    //the entirety of the left map is copied over except for the rightmost two columns which are truncated by the right map
                    while (leftReader.Peek() >= 0)
                    {
                        string line = leftReader.ReadLine()!;
                        string[] elements = line.Split(';');
                        if (Convert.ToInt32(elements[2]) < leftWidth - 2)
                        {
                            streamWriter.WriteLine(line);
                        }
                    }

                    while (rightReader.Peek() >= 0)
                    {
                        string line = rightReader.ReadLine()!;
                        string[] elements = line.Split(";");
                        if (Convert.ToInt32(elements[2]) == 0 && elements[0] == "T")
                        {
                            //convert the 0th tile of each row of the right map from:
                            //T;y;x;4;4;4;4;8;0 (for even rows)
                            //T;y;x;4;0;4;0;8;0 (for odd rows)
                            //into
                            //T;y;x;0;0;0;0;8;0 the '8' currently has an unknown effect 
                            //but usually non-boundary tiles will have a '0' instead
                            //leaving the '8' does not affect map loading
                            for (int i = 3; i < 7; i++)
                            {
                                elements[i] = "0";
                            }
                            elements[2] = Convert.ToString(Convert.ToInt32(elements[2]) + leftWidth - 2);
                            streamWriter.WriteLine(string.Join(";", elements));
                        }
                        else
                        {
                            elements[2] = Convert.ToString(Convert.ToInt32(elements[2]) + leftWidth - 2);
                            streamWriter.WriteLine(string.Join(";", elements));
                        }
                    }
                    //TODO: handle logic for when new edges have different elevations
                    CheckSeams(targetPath, streamWriter, true);

                    leftReader.Dispose();
                    rightReader.Dispose();
                    streamWriter.Dispose();
                    MessageBox.Show("Map succesfully created.", "Success", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("Invalid new map name. You cannot use these characters when naming a new map:\n" + new String(Path.GetInvalidFileNameChars()), "Error", MessageBoxButtons.OK);
                }
            }
            else
            {
                MessageBox.Show("Check to make sure you have selected two maps and have entered a new name in the text box.", "Error", MessageBoxButtons.OK);
            }
        }

        private void AddToTop(object sender, EventArgs e)
        {
            if (AreTextBoxesNotNull())
            {
                if (ValidFileName(newMapName.Text))
                {
                    StreamReader topReader = new(mapPath2);
                    StreamReader bottomReader = new(mapPath1);

                    //sets the target directory for the writer to the 'map builder' folder in the repo
                    //TODO: change so that, when ran from the SF folder, the target is the custom maps folder
                    string targetPath = Path.Combine(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName, "map builder"), newMapName.Text + ".map");
                    StreamWriter streamWriter = new(targetPath);
                    string[] topFirstLine = topReader.ReadLine()!.Split(';');
                    //first line of every map file has the form
                    //Map;mapName;number of maps made in session(irrelevant);height;width;unknownx7;series of dozens of semicolons for unknown reason
                    int topHeight = Convert.ToInt32(topFirstLine[3]);
                    int bottomHeight = Convert.ToInt32(bottomReader.ReadLine()!.Split(";")[3]);

                    topFirstLine[1] = newMapName.Text;
                    topFirstLine[4] = Convert.ToString(topHeight + bottomHeight - 2);

                    streamWriter.WriteLine(string.Join(';', topFirstLine));

                    //the entirety of the top map is copied over except for the bottommost two rows which are truncated by the bottom map
                    while (topReader.Peek() >= 0)
                    {
                        string line = topReader.ReadLine()!;
                        string[] elements = line.Split(';');
                        if (Convert.ToInt32(elements[1]) < topHeight - 2)
                        {
                            streamWriter.WriteLine(line);
                        }
                    }

                    while (bottomReader.Peek() >= 0)
                    {
                        string line = bottomReader.ReadLine()!;
                        string[] elements = line.Split(";");
                        if (Convert.ToInt32(elements[1]) == 0 && elements[0] == "T")
                        {
                            //convert the 0th tile of each column of the bottom map from:
                            //T;y;x;4;4;0;0;8;0 
                            //into
                            //T;y;x;0;0;0;0;8;0 the '8' currently has an unknown effect 
                            //but usually non-boundary tiles will have a '0' instead
                            //leaving the '8' does not affect map loading
                            for (int i = 3; i < 5; i++)
                            {
                                elements[i] = "0";
                            }
                            elements[1] = Convert.ToString(Convert.ToInt32(elements[1]) + topHeight - 2);
                            streamWriter.WriteLine(string.Join(";", elements));
                        }
                        else
                        {
                            elements[1] = Convert.ToString(Convert.ToInt32(elements[1]) + topHeight - 2);
                            streamWriter.WriteLine(string.Join(";", elements));
                        }
                    }
                    //TODO: handle logic for when new edges have different elevations
                    CheckSeams(targetPath, streamWriter, false);

                    topReader.Dispose();
                    bottomReader.Dispose();
                    streamWriter.Dispose();
                    MessageBox.Show("Map succesfully created.", "Success", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("Invalid new map name. You cannot use these characters when naming a new map:\n" + new String(Path.GetInvalidFileNameChars()), "Error", MessageBoxButtons.OK);
                }
            }
            else
            {
                MessageBox.Show("Check to make sure you have selected two maps and have entered a new name in the text box.", "Error", MessageBoxButtons.OK);
            }
        }

        private void AddToBottom(object sender, EventArgs e)
        {
            if (AreTextBoxesNotNull())
            {
                if (ValidFileName(newMapName.Text))
                {
                    StreamReader topReader = new(mapPath1);
                    StreamReader bottomReader = new(mapPath2);

                    //sets the target directory for the writer to the 'map builder' folder in the repo
                    //TODO: change so that, when ran from the SF folder, the target is the custom maps folder
                    string targetPath = Path.Combine(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName, "map builder"), newMapName.Text + ".map");
                    StreamWriter streamWriter = new(targetPath);
                    string[] topFirstLine = topReader.ReadLine()!.Split(';');
                    //first line of every map file has the form
                    //Map;mapName;number of maps made in session(irrelevant);height;width;unknownx7;series of dozens of semicolons for unknown reason
                    int topHeight = Convert.ToInt32(topFirstLine[3]);
                    int bottomHeight = Convert.ToInt32(bottomReader.ReadLine()!.Split(";")[3]);

                    topFirstLine[1] = newMapName.Text;
                    topFirstLine[4] = Convert.ToString(topHeight + bottomHeight - 2);

                    streamWriter.WriteLine(string.Join(';', topFirstLine));

                    //the entirety of the top map is copied over except for the bottommost two rows which are truncated by the bottom map
                    while (topReader.Peek() >= 0)
                    {
                        string line = topReader.ReadLine()!;
                        string[] elements = line.Split(';');
                        if (Convert.ToInt32(elements[1]) < topHeight - 2)
                        {
                            streamWriter.WriteLine(line);
                        }
                    }

                    while (bottomReader.Peek() >= 0)
                    {
                        string line = bottomReader.ReadLine()!;
                        string[] elements = line.Split(";");
                        if (Convert.ToInt32(elements[1]) == 0 && elements[0] == "T")
                        {
                            //convert the 0th tile of each column of the bottom map from:
                            //T;y;x;4;4;0;0;8;0 
                            //into
                            //T;y;x;0;0;0;0;8;0 the '8' currently has an unknown effect 
                            //but usually non-boundary tiles will have a '0' instead
                            //leaving the '8' does not affect map loading
                            for (int i = 3; i < 5; i++)
                            {
                                elements[i] = "0";
                            }
                            elements[1] = Convert.ToString(Convert.ToInt32(elements[1]) + topHeight - 2);
                            streamWriter.WriteLine(string.Join(";", elements));
                        }
                        else
                        {
                            elements[1] = Convert.ToString(Convert.ToInt32(elements[1]) + topHeight - 2);
                            streamWriter.WriteLine(string.Join(";", elements));
                        }
                    }
                    //TODO: handle logic for when new edges have different elevations
                    CheckSeams(targetPath, streamWriter, false);

                    topReader.Dispose();
                    bottomReader.Dispose();
                    streamWriter.Dispose();
                    MessageBox.Show("Map succesfully created.", "Success", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("Invalid new map name. You cannot use these characters when naming a new map:\n" + new String(Path.GetInvalidFileNameChars()), "Error", MessageBoxButtons.OK);
                }
            }
            else
            {
                MessageBox.Show("Check to make sure you have selected two maps and have entered a new name in the text box.", "Error", MessageBoxButtons.OK);
            }
        }
    }
}