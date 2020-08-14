/*******************************************************************************
 *Author: Kien Truong
 ******************************************************************************/

using System;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using System.Collections.Generic;

public class RicochetBall: Form
{
  private const int maxFormWidth = 1920;
  private const int maxFormHeight = 1080;
  private const int minFormWidth = 640;
  private const int minFormHeight = 360;
  Size maxFrameSize = new Size(maxFormWidth,maxFormHeight);
  Size minFrameSize = new Size(minFormWidth,minFormHeight);

  private const int topPanelHeight    = 50;
  private const int bottomPanelHeight = 110;

  //Declare data about the ball
  private const double radius = 8.5;
  private double ball_linear_speed_pix_per_sec;
  private double ball_linear_speed_pix_per_tic;
  private double speed;
  private double angle;
  private double ball_delta_x;
  private double ball_delta_y;
  //Ball initial position when the program is not running and when "Refresh"
  //button is clicked by the user
  private const double ball_center_initial_coord_x = (double)1280*0.5;
  private const double ball_center_initial_coord_y = (double)480/2.0 + topPanelHeight;
  private double ball_center_current_coord_x;
  private double ball_center_current_coord_y;
  private double ball_upper_left_current_coord_x;
  private double ball_upper_left_current_coord_y;
  private double radian;

  private const String welcome_message = "Ricochet Ball by Kien Truong";
  private System.Drawing.Font welcome_style = new System.Drawing.Font("TimesNewRoman",24,FontStyle.Regular);
  private Brush welcome_paint_brush = new SolidBrush(System.Drawing.Color.Black);
  private Point welcome_location;   //Will be initialized in the constructor.

  private Button newButton   = new Button();
  private Button startButton = new Button();
  private Button quitButton  = new Button();
  private Label  xCenterBall = new Label();
  private Label  yCenterBall = new Label();
  private Label  speedLabel  = new Label();
  private Label  angleLabel  = new Label();

  private TextBox speedInput      = new TextBox();
  private TextBox directionInput  = new TextBox();

  private int formWidth;
  private int formHeight;

  //Declare data about the motion clock
  private static System.Timers.Timer ball_motion_control_clock = new System.Timers.Timer();
  private const double ball_motion_control_clock_rate = 20;  //Units are Hz

  //Declare data about the refresh clock;
  private static System.Timers.Timer graphic_area_refresh_clock = new System.Timers.Timer();
  private const double graphic_refresh_rate = 23.3;  //Units are Hz = #refreshes per second

  public RicochetBall()
  {//Set the size of the user interface box.
    formWidth = (maxFormWidth+minFormWidth)/2;
    formHeight = (maxFormHeight+minFormHeight)/2;
    Size = new Size(formWidth,formHeight);

    MaximumSize = maxFrameSize;
    MinimumSize = minFrameSize;

    //Initialize text strings
    Text = "Ricochet Ball by Kien Truong";
    System.Console.WriteLine("Form_width = {0}, Form_height = {1}.", Width, Height);
    newButton.Text      = "New";
    startButton.Text    = "Start";
    quitButton.Text     = "Quit";
    speedLabel.Text     = "Speed";
    angleLabel.Text     = "Angle";

    //Set sizes
    newButton.Size      = new Size(100,40);
    startButton.Size    = new Size(100,40);
    quitButton.Size     = new Size(100,40);
    xCenterBall.Size    = new Size(100,40);
    yCenterBall.Size    = new Size(100,40);
    directionInput.Size = new Size(100,60);
    speedInput.Size     = new Size(100,60);
    speedLabel.Size     = new Size(100,20);
    angleLabel.Size     = new Size(100,20);

    //Set locations
    newButton.Location      = new Point(50,550);
    startButton.Location    = new Point(400,550);
    quitButton.Location     = new Point(750,550);
    xCenterBall.Location    = new Point(1100,560);
    yCenterBall.Location    = new Point(1100,630);
    speedInput.Location     = new Point(400,650);
    directionInput.Location = new Point(680, 650);
    speedLabel.Location     = new Point(400,600);
    angleLabel.Location     = new Point(680,600);

    //Set colors
    this.BackColor          = Color.Green;
    newButton.BackColor     = Color.Yellow;
    startButton.BackColor   = Color.Yellow;
    quitButton.BackColor    = Color.Yellow;
    xCenterBall.BackColor   = Color.White;
    yCenterBall.BackColor   = Color.White;
    speedLabel.BackColor    = Color.White;
    angleLabel.BackColor    = Color.White;

    //Add controls to the form
    Controls.Add(newButton);
    Controls.Add(startButton);
    Controls.Add(quitButton);
    Controls.Add(xCenterBall);
    Controls.Add(yCenterBall);
    Controls.Add(speedInput);
    Controls.Add(directionInput);
    Controls.Add(speedLabel);
    Controls.Add(angleLabel);

    welcome_location = new Point(Width/2-250,8);

    ball_center_current_coord_x = ball_center_initial_coord_x;
    ball_center_current_coord_y = ball_center_initial_coord_y;

    //Register the event handler.  In this case each button has an event handler, but no other
    //controls have event handlers.
    newButton.Enabled       = true;
    startButton.Enabled     = true;
    quitButton.Enabled      = true;

    //Set up the motion clock.  This clock controls the rate of update of the coordinates of the ball.
    ball_motion_control_clock.Enabled = false;
    //Assign a handler to this clock to update the ball position.
    ball_motion_control_clock.Elapsed += new ElapsedEventHandler(Update_ball_position);

    //Set up the refresh clock.
    graphic_area_refresh_clock.Enabled = false;  //Initially the clock controlling the rate of updating the display is stopped.
    //Assign a handler to this clock.
    graphic_area_refresh_clock.Elapsed += new ElapsedEventHandler(Update_display);

    //Use extra memory to make a smooth animation.
    DoubleBuffered = true;

    startButton.Click += new EventHandler(compute);
    quitButton.Click  += new EventHandler(stoprun);  //The '+' is required.
    newButton.Click   += new EventHandler(refresh);
  }//End of constructor TrafficSignal

//Method to execute when the exit button receives an event, namely: receives a mouse click
  protected void stoprun(Object sender, EventArgs events)
  {
    Close();
  }//End of stoprun

  protected void refresh(Object sender, EventArgs events)
  {
    ball_motion_control_clock.Enabled  = false;
    graphic_area_refresh_clock.Enabled = false;
    speedInput.Text     = " ";
    directionInput.Text = " ";
    ball_center_current_coord_x = (double)1280*0.5;
    ball_center_current_coord_y = (double)480/2.0 + topPanelHeight;
    speed = 0.0;
    angle = 0.0;
    Invalidate();  //This will call OnPaint
  }//END refresh

  protected void compute(Object sender, EventArgs events)
  {
    speed = double.Parse(speedInput.Text);
    angle = double.Parse(directionInput.Text);

    radian = (Math.PI/180.0) * angle;

    ball_linear_speed_pix_per_tic = speed/ball_motion_control_clock_rate;
    ball_delta_y = ball_linear_speed_pix_per_tic*System.Math.Sin(radian);
    ball_delta_x = ball_linear_speed_pix_per_tic*System.Math.Cos(radian);

    Start_graphic_clock(graphic_refresh_rate);
    //The motion clock is started.
    Start_ball_clock(ball_motion_control_clock_rate);
  }//END compute

  protected void Start_graphic_clock(double refresh_rate)
  {
   double actual_refresh_rate = 1.0;  //Minimum refresh rate is 1 Hz to avoid a potential division by a number close to zero
   double elapsed_time_between_tics;

   if(refresh_rate > actual_refresh_rate)
   {
     actual_refresh_rate = refresh_rate;
   }
   elapsed_time_between_tics = 1000.0/actual_refresh_rate;  //elapsedtimebetweentics has units milliseconds.
   graphic_area_refresh_clock.Interval = (int)System.Math.Round(elapsed_time_between_tics);
   graphic_area_refresh_clock.Enabled = true;  //Start clock ticking.
 }//END Start_graphic_clock

  protected void Start_ball_clock(double update_rate)
  {
    double elapsed_time_between_ball_moves;
    if(update_rate < 1.0) update_rate = 1.0;  //This program does not allow updates slower than 1 Hz.

    elapsed_time_between_ball_moves = 1000.0/update_rate;  //1000.0ms = 1second.
    //The variable elapsed_time_between_ball_moves has units "milliseconds".
    ball_motion_control_clock.Interval = (int)System.Math.Round(elapsed_time_between_ball_moves);
    ball_motion_control_clock.Enabled = true;   //Start clock ticking.
  }//END Start_ball_clock

  protected void Update_display(System.Object sender, ElapsedEventArgs evt)
  {
    Invalidate();  //This creates an artificial event so that the graphic area will repaint itself.
    //System.Console.WriteLine("The motion clock ticked and the time is {0}", evt.SignalTime);  //Debug statement; remove it later.
    if(!ball_motion_control_clock.Enabled)
    {
      graphic_area_refresh_clock.Enabled = false;
      System.Console.WriteLine("The graphical area is no longer refreshing.  You may close the window.");
    }
  }//END Update_display

  protected void Update_ball_position(System.Object sender, ElapsedEventArgs evt)
  {
    ball_center_current_coord_x += ball_delta_x;
    ball_center_current_coord_y -= ball_delta_y;  //The minus sign is due to the upside down nature of the C# system.
    //System.Console.WriteLine("The motion clock ticked and the time is {0}", evt.SignalTime);//Debug statement; remove later.
    //Determine if the ball has made a collision with the right wall.
    if((int)System.Math.Round(ball_center_current_coord_x + radius) >= formWidth)
         ball_delta_x = -ball_delta_x;
    //Determine if the ball has made a collision with the lower wall
    if((int)System.Math.Round(ball_center_current_coord_y + radius) >= 530)
         ball_delta_y = -ball_delta_y;
    //Determine if the ball has made a collision with the left wall
    if((int)System.Math.Round(ball_center_current_coord_x - radius) <= 0)
         ball_delta_x = -ball_delta_x;

    //Determine if the ball has made a collision with the upper wall
    if((int)System.Math.Round(ball_center_current_coord_y - radius) <= topPanelHeight)
         ball_delta_y = -ball_delta_y;

    xCenterBall.Text = ((int)ball_center_current_coord_x).ToString();
    yCenterBall.Text = ((int)ball_center_current_coord_y).ToString();

    //The next statement checks to determine if the ball has traveled beyond the four boundaries.
    // if((int)System.Math.Round(ball_center_current_coord_y - radius) >= 480+topPanelHeight)
    //   {ball_motion_control_clock.Enabled = false;
    //    graphic_area_refresh_clock.Enabled = false;
    //    System.Console.WriteLine("The clock controlling the ball has stopped.");
    //   }
  }//End of method Update_ball_position

  protected override void OnPaint(PaintEventArgs ee)
  {
    Graphics lights = ee.Graphics;
    Pen blackPen = new Pen(Color.Black, 3);

    lights.FillRectangle(Brushes.Blue,0,0,Width,topPanelHeight);
    lights.DrawString(welcome_message,welcome_style,welcome_paint_brush,welcome_location);

    lights.FillRectangle(Brushes.Brown,0,530,Width,topPanelHeight+130);

    ball_upper_left_current_coord_x = ball_center_current_coord_x - radius;
    ball_upper_left_current_coord_y = ball_center_current_coord_y - radius;
    lights.FillEllipse(Brushes.Red,(int)ball_upper_left_current_coord_x,(int)ball_upper_left_current_coord_y,
                      (float)(2.0*radius),(float)(2.0*radius));

    base.OnPaint(ee);
  }//END protected override void OnPaint(PaintEventArgs ee)

}//End of class RicochetBall
