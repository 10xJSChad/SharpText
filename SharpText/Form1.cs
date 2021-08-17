using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace SharpText
{
    public partial class Main : Form
    {
        bool Command_Mode = false;

        string command = "";
        string prompt = "";
        string file = "";

        int Selected_Line = 0;
        int Display_From = 0;
        List<string> Loaded_Lines = new List<string>() {};
        List<string> To_Display = new List<string>() { "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

        public Main()
        {
            InitializeComponent();

        }
        void Update_Display_List()
        {
            int temp = Display_From;
            for(int i = 0; i < 14; i++)
            {
                if(temp < Loaded_Lines.Count) To_Display[i] = Loaded_Lines[temp];
                else To_Display[i] = "";
                temp++;
            }
        }

        void Render_Text()
        {
            line1.Text = To_Display[0];
            line2.Text = To_Display[1];
            line3.Text = To_Display[2];
            line4.Text = To_Display[3];
            line5.Text = To_Display[4];
            line6.Text = To_Display[5];
            line7.Text = To_Display[6];
            line8.Text = To_Display[7];
            line9.Text = To_Display[8];
            line10.Text = To_Display[9];
            line11.Text = To_Display[10];
            line12.Text = To_Display[11];
            line13.Text = To_Display[12];
            line14.Text = To_Display[13];
        }

        void Render_Text_CommandMode(string newPrompt = null)
        {
            if(newPrompt != null) prompt = newPrompt;
            line1.Text = prompt;
            line2.Text = command;
            line3.Text = "";
            line4.Text = "";
            line5.Text = "";
            line6.Text = "";
            line7.Text = "";
            line8.Text = "";
            line9.Text = "";
            line10.Text = "";
            line11.Text = "";
            line12.Text = "";
            line13.Text = "";
            line14.Text = "";

            line1.ForeColor = Color.Lime;
            line2.ForeColor = Color.Lime;
        }

        void Render_Selected()
        {
            Color Selected_Color = Color.Lime;
            Color Default_Color = Color.White;
            if (Selected_Line == 0) line1.ForeColor = Selected_Color;
            else line1.ForeColor = Default_Color;
            if (Selected_Line == 1) line2.ForeColor = Selected_Color;
            else line2.ForeColor = Default_Color;
            if (Selected_Line == 2) line3.ForeColor = Selected_Color;
            else line3.ForeColor = Default_Color;
            if (Selected_Line == 3) line4.ForeColor = Selected_Color;
            else line4.ForeColor = Default_Color;
            if (Selected_Line == 4) line5.ForeColor = Selected_Color;
            else line5.ForeColor = Default_Color;
            if (Selected_Line == 5) line6.ForeColor = Selected_Color;
            else line6.ForeColor = Default_Color;
            if (Selected_Line == 6) line7.ForeColor = Selected_Color;
            else line7.ForeColor = Default_Color;
            if (Selected_Line == 7) line8.ForeColor = Selected_Color;
            else line8.ForeColor = Default_Color;
            if (Selected_Line == 8) line9.ForeColor = Selected_Color;
            else line9.ForeColor = Default_Color;
            if (Selected_Line == 9) line10.ForeColor = Selected_Color;
            else line10.ForeColor = Default_Color;
            if (Selected_Line == 10) line11.ForeColor = Selected_Color;
            else line11.ForeColor = Default_Color;
            if (Selected_Line == 11) line12.ForeColor = Selected_Color;
            else line12.ForeColor = Default_Color;
            if (Selected_Line == 12) line13.ForeColor = Selected_Color;
            else line13.ForeColor = Default_Color;
            if (Selected_Line == 13) line14.ForeColor = Selected_Color;
            else line14.ForeColor = Default_Color;
        }

        void Load_File(string file_name)
        {
            file = file_name;
            if (!File.Exists(file_name)) { var new_file = File.Create(file_name); new_file.Close(); Load_File(file_name);  return; }
            string[] lines = System.IO.File.ReadAllLines(@file_name);
            Command_Mode = false;
            foreach (string line in lines)
            {
                Loaded_Lines.Add(line);
            }
            Enter_EditMode();
        }

        void Enter_CommandMode()
        {
            Command_Mode = true;
            Render_Text_CommandMode("Open or create file");
        }

        void Enter_EditMode()
        {
            Render_Selected();
            Update_Display_List();
            Render_Text();
        }

        void Save_File()
        {
            TextWriter tw = new StreamWriter(file);
            foreach (String s in Loaded_Lines)
                tw.WriteLine(s);
            tw.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Enter_CommandMode();        
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (Command_Mode) return;
            if (e.KeyCode == Keys.Up)
            {
                if (Selected_Line > 0) Selected_Line--;
                else
                {
                    if (Display_From - 1 < 0) return;
                    Display_From--;
                    Update_Display_List();
                    Render_Text();
                }
                Render_Selected();
                return;
            }

            if (e.KeyCode == Keys.Down)
            {
                if (Selected_Line < 13) Selected_Line++;
                else
                {
                    if (Display_From + 14 > Loaded_Lines.Count) return;
                    Display_From++;
                    Update_Display_List();
                    Render_Text();
                }
                if (Selected_Line + Display_From >= Loaded_Lines.Count) Loaded_Lines.Add("");
                Render_Selected();
                return;
            }
        }

        private void Main_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Command_Mode)
            {
                if ((int)e.KeyChar == 13) { Load_File(command); return; }
                if ((int)e.KeyChar == 8)
                {
                    if(command.Length > 0) command = command.Remove(command.Length - 1, 1);
                    Render_Text_CommandMode();
                    return;
                }
                command += e.KeyChar;
                Render_Text_CommandMode();
                return;
            }

            if ((int)e.KeyChar == 8)
            {
                if (Loaded_Lines[Selected_Line + Display_From].Length > 0)
                {
                    Loaded_Lines[Selected_Line + Display_From] = Loaded_Lines[Selected_Line + Display_From].Remove(Loaded_Lines[Selected_Line + Display_From].Length - 1, 1);
                }
                else
                {
                    if (Selected_Line - 1 >= 0) Selected_Line--;
                    Loaded_Lines.RemoveAt(Selected_Line + Display_From + 1);
                }
                Render_Selected();
                Update_Display_List();
                Render_Text();
                return;
            }

            if ((int)e.KeyChar == 13)
            {
                if (Selected_Line + 1 < 13) Selected_Line++;
                Loaded_Lines.Insert(Selected_Line + Display_From, "");
                Render_Selected();
                Update_Display_List();
                Render_Text();
                return;
            }

            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                if ((int)e.KeyChar == 19) { Save_File(); return; }
            }

            if (Selected_Line >= Loaded_Lines.Count) Loaded_Lines.Add("");
            Loaded_Lines[Selected_Line + Display_From] += e.KeyChar;
            To_Display[Selected_Line] += e.KeyChar;
            Render_Text();
        }
    }
}
