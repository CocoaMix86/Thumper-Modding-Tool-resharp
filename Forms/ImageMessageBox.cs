using System.Windows.Forms;

namespace Thumper_Modding_Tool_resharp
{
    public partial class ImageMessageBox : Form
	{
		private System.Drawing.Size _size;

		public ImageMessageBox()
		{
			InitializeComponent();
		}

		public ImageMessageBox(string path)
		{
			if (path == "difficultyhelp") {
				this.BackgroundImage = Properties.Resources.difficultyhelp;
				this.BackgroundImageLayout = ImageLayout.Stretch;
				this.Text = "Difficulty Explanation";
				_size = new System.Drawing.Size(700, 690);
			}
			this.Size = _size;
			this.Height += 40;
		}
	}
}
