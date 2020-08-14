/*******************************************************************************
 *Author: Kien Truong
 ******************************************************************************/

using System;
using System.Windows.Forms;  //Needed for "Application" on next to last line of Main
public class RicochetMain
{
   static void Main(string[] args)
   {//System.Console.WriteLine("Welcome to the Main method of the Fibonacci program.");
    RicochetBall ricochetBallApp = new RicochetBall();
    Application.Run(ricochetBallApp);
    System.Console.WriteLine("Main method will now shutdown.");
   }//End of Main
}//End of Fibonaccimain
