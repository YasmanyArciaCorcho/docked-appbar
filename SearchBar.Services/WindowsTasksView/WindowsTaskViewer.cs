using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace Service.WindowsTasksView
{
    public class WindowsTaskViewer : IWindowsTaskViewer
    {
        public void ShowWindowsTaskView()
        {
            WindowsInput.InputSimulator kb = new WindowsInput.InputSimulator();
            kb.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.TAB);
        }
    }
}
