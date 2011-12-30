using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using texforge_definitions.Settings;
using System.Windows.Forms;
using texforge_definitions;
using System.Drawing;
using texforge.Graph;
using System.IO;

namespace texforge
{
    public class SettingComponentFactory
    {
        public static void CreateComponent(SettingBase setting, VisualGraph.DraggableObject owner, FlowLayoutPanel panel, PictureBox render)
        {
            SettingComponent component = null;
            switch (setting.GetType().Name)
            {
                case "Filename":
                    component = new FilenameSettingComponent(setting, owner, render);
                    break;
                case "String":
                    component = new StringSettingComponent(setting, owner, render);
                    break;
                case "Color":
                    component = new ColorSettingComponent(setting, owner, render);
                    break;
                case "Int":
                    component = new IntSettingComponent(setting, owner, render);
                    break;
                case "Float":
                    component = new FloatSettingComponent(setting, owner, render);
                    break;
                case "Bool":
                    component = new BoolSettingComponent(setting, owner, render);
                    break;
                default:
                    component = new InvalidSettingComponent(setting, owner, render);
                    break;
            }
            // Special handling for enums
            if (setting.GetType().IsSubclassOf(typeof(Enumeration)))
            {
                component = new EnumSettingComponent(setting, owner, render);
            }
            panel.Controls.Add(component.Container);
        }

        abstract class SettingComponent
        {
            protected Panel container;
            protected VisualGraph.DraggableObject owner;
            protected PictureBox render;
            protected List<Control> refresh = new List<Control>();
            public Panel Container
            {
                get { return container; }
            }

            public SettingComponent(VisualGraph.DraggableObject owner, PictureBox render)
            {
                container = new Panel();
                container.Dock = DockStyle.Bottom;
                container.Tag = this;
                this.owner = owner;
                this.render = render;
                this.container.Height = 50;
            }

            protected virtual void ValueChanged()
            {
                owner.Dirty = true;
                foreach (Control control in refresh)
                    control.Invalidate();
                render.Invalidate();
            }

            protected Panel CreateDefaultGroupBox(string name)
            {
                GroupBox box = new GroupBox();
                box.Text = name;
                box.Dock = DockStyle.Fill;
                Panel panel = new Panel();
                panel.Dock = DockStyle.Fill;
                box.Controls.Add(panel);
                container.Controls.Add(box);
                return panel;
            }
        }

        class InvalidSettingComponent : SettingComponent
        {
            public InvalidSettingComponent(SettingBase setting, VisualGraph.DraggableObject owner, PictureBox render)
                : base(owner, render)
            {
                Label invalid = new Label();
                invalid.Text = "Unsupported type: " + setting.GetType().Name;
                invalid.Dock = DockStyle.Fill;
                container.Controls.Add(invalid);
            }
        }
         
        class ColorSettingComponent : SettingComponent
        {
            texforge_definitions.Settings.Color data;

            public ColorSettingComponent(SettingBase setting, VisualGraph.DraggableObject owner, PictureBox render)
                : base(owner, render)
            {
                data = (texforge_definitions.Settings.Color)setting;
                Panel panel = CreateDefaultGroupBox(data.Name);
                PictureBox color = new PictureBox();
                color.Dock = DockStyle.Fill;
                panel.Controls.Add(color);
                color.Paint += new PaintEventHandler(color_Paint);
                color.Click += new EventHandler(change_Click);
                refresh.Add(color);
            }

            void change_Click(object sender, EventArgs e)
            {
                ColorDialog choose = new ColorDialog();
                DialogResult choice = choose.ShowDialog();
                if (choice == DialogResult.OK)
                {
                    data.Value = new texforge_definitions.Types.Color(choose.Color);
                    ValueChanged();
                }
            }

            void color_Paint(object sender, PaintEventArgs e)
            {
                e.Graphics.FillRectangle(new SolidBrush(data.Value.WindowsColor), e.ClipRectangle);
            }

        }

        class EnumSettingComponent : SettingComponent
        {
            texforge_definitions.Settings.Enumeration data;
            Dictionary<int, string> items = new Dictionary<int, string>();

            public EnumSettingComponent(SettingBase setting, VisualGraph.DraggableObject owner, PictureBox render)
                : base(owner, render)
            {
                data = (texforge_definitions.Settings.Enumeration)setting;
                Panel panel = CreateDefaultGroupBox(data.Name);
                ComboBox mode = new ComboBox();
                foreach( string item in data.AvailableValues)
                {
                    int index = mode.Items.Add(item);
                    items[index] = item;
                    if (item == data.Value)
                        mode.SelectedIndex = index;
                }
                panel.Controls.Add(mode);
                mode.SelectedIndexChanged += new EventHandler(mode_SelectedIndexChanged);
            }

            void mode_SelectedIndexChanged(object sender, EventArgs e)
            {
                data.Value = items[((ComboBox)sender).SelectedIndex];
                ValueChanged();
            }
        }

        class IntSettingComponent : SettingComponent
        {
            texforge_definitions.Settings.Int data;

            public IntSettingComponent(SettingBase setting, VisualGraph.DraggableObject owner, PictureBox render)
                : base(owner, render)
            {
                data = (texforge_definitions.Settings.Int)setting;
                Panel panel = CreateDefaultGroupBox(data.Name);
                TextBox input = new TextBox();
                input.Text = data.Value.ToString();
                panel.Controls.Add(input);
                input.TextChanged += new EventHandler(input_TextChanged);
            }

            void input_TextChanged(object sender, EventArgs e)
            {
                int newValue;
                if (int.TryParse(((TextBox)sender).Text, out newValue))
                {
                    data.Value = newValue;
                    ValueChanged();
                }
            }
        }

        class StringSettingComponent : SettingComponent
        {
            texforge_definitions.Settings.String data;

            public StringSettingComponent(SettingBase setting, VisualGraph.DraggableObject owner, PictureBox render)
                : base(owner, render)
            {
                data = (texforge_definitions.Settings.String)setting;
                Panel panel = CreateDefaultGroupBox(data.Name);
                TextBox input = new TextBox();
                input.Text = data.Value.ToString();
                panel.Controls.Add(input);
                input.TextChanged += new EventHandler(input_TextChanged);
            }

            void input_TextChanged(object sender, EventArgs e)
            {
                data.Value = ((TextBox)sender).Text;
                ValueChanged();
            }
        }

        class FilenameSettingComponent : SettingComponent
        {
            texforge_definitions.Settings.Filename data;

            TextBox input;

            public FilenameSettingComponent(SettingBase setting, VisualGraph.DraggableObject owner, PictureBox render)
                : base(owner, render)
            {
                data = (texforge_definitions.Settings.Filename)setting;
                Panel panel = CreateDefaultGroupBox(data.Name);
                input = new TextBox();
                input.Text = data.Value.ToString();
                panel.Controls.Add(input);
                Button browse = new Button();
                browse.Text = "...";
                panel.Controls.Add(browse);
                browse.Click += new EventHandler(browse_Click);
                input.TextChanged += new EventHandler(input_TextChanged);
            }            

            void browse_Click(object sender, EventArgs e)
            {
                using (OpenFileDialog fd = new OpenFileDialog())
                {
                    if (fd.ShowDialog() == DialogResult.OK)
                    {
                        input.Text = fd.FileName;
                        ValueChanged();
                    }
                }
            }

            protected override void ValueChanged()
            {
                if (File.Exists(input.Text))
                {
                    base.ValueChanged();
                }
            }

            void input_TextChanged(object sender, EventArgs e)
            {
                data.Value = ((TextBox)sender).Text;
                ValueChanged();
            }
        }

        class FloatSettingComponent : SettingComponent
        {
            texforge_definitions.Settings.Float data;

            public FloatSettingComponent(SettingBase setting, VisualGraph.DraggableObject owner, PictureBox render)
                : base(owner, render)
            {
                data = (texforge_definitions.Settings.Float)setting;
                Panel panel = CreateDefaultGroupBox(data.Name);
                TextBox input = new TextBox();
                input.Text = data.Value.ToString();
                panel.Controls.Add(input);
                input.TextChanged += new EventHandler(input_TextChanged);
            }

            void input_TextChanged(object sender, EventArgs e)
            {
                float newValue;
                if (float.TryParse(((TextBox)sender).Text, out newValue))
                {
                    data.Value = newValue;
                    ValueChanged();
                }
            }
        }

        class BoolSettingComponent : SettingComponent
        {
            texforge_definitions.Settings.Bool data;

            public BoolSettingComponent(SettingBase setting, VisualGraph.DraggableObject owner, PictureBox render)
                : base(owner, render)
            {
                data = (texforge_definitions.Settings.Bool)setting;
                Panel panel = CreateDefaultGroupBox(data.Name);
                CheckBox value = new CheckBox();
                value.Checked = data.Value;
                panel.Controls.Add(value);
                value.CheckedChanged += new EventHandler(value_CheckedChanged);
            }

            void value_CheckedChanged(object sender, EventArgs e)
            {
                data.Value = ((CheckBox)sender).Checked;
                ValueChanged();
            }

        }
    }

}
