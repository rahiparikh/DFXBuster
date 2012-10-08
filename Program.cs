/*
 * 
 *  Project     : DFX Buster
 *  File        : Program.cs
 *  Developer   : Rahil Parikh ( rahil@rahilparikh.me )
 *  Date        : Sept 17, 2012
 *  
 *  Copyright (c) 2012, Rahil Parikh
 *
 *  As long as you retain this notice and credit author
 *  for his work you can do whatever you want with this
 *  stuff. 
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *  THE SOFTWARE.
 *  
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DFX_Buster
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmDFX_Buster());
        }
    }
}
