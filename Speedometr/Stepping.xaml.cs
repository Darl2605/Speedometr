namespace Speedometr;

public partial class Stepping : ContentPage
{
    int Goal;
    bool SetBar;
    private const float StepThreshold = 0.96f;
    private float previousMagnitutde = 0;
    int steps = -1;

    public Stepping()
	{
		InitializeComponent();
        Start();
	}

    void Start() => ToggleAccelerometer();

    private void SetSlider()
    {
        if (SetBar)
        {
          Progressbar.Minimum = 0;
          Progressbar.Maximum = Goal;
          SetBar = false;
        }
        Progressbar.Value = steps / 2;
    }

    public void ToggleAccelerometer()
    {
        if (Accelerometer.Default.IsSupported)
        {
            if (!Accelerometer.Default.IsMonitoring)
            {
                // Turn on accelerometer
                Accelerometer.Default.ReadingChanged += Accelerometer_ReadingChanged;
                Accelerometer.Default.Start(SensorSpeed.UI);

            }
            else
            {
                // Turn off accelerometer
                Accelerometer.Default.Stop();
                Accelerometer.Default.ReadingChanged -= Accelerometer_ReadingChanged;
            }
        }
    }

    public void Accelerometer_ReadingChanged(object? sender, AccelerometerChangedEventArgs e)
    {
        Tilt.Text = $"X:{Math.Round(e.Reading.Acceleration.X,2)}\n Y:{Math.Round(e.Reading.Acceleration.Y,2)} \n Z:{Math.Round(e.Reading.Acceleration.Z, 2)}";
        var acceleration = e.Reading;
        var magnitude = Math.Sqrt(
            acceleration.Acceleration.X * acceleration.Acceleration.X +
            acceleration.Acceleration.Y * acceleration.Acceleration.Y +
            acceleration.Acceleration.Z * acceleration.Acceleration.Z);

        if (Math.Abs(magnitude - previousMagnitutde) > StepThreshold)
        {
            steps++;
        }

        previousMagnitutde = (float)magnitude;
        SetSlider();
        Remaining_Steps.Text = $"Zbývá {Remaining(steps)} krokù";
        Current_steps.Text = $"Ušel jsi {(double)steps / 2} krokù";

        

    }


    string Remaining(int steps)
    {
        string zbyva = Goal > 0 ? $"{Goal - (steps / 2)}" : "Není";
        return zbyva;
    }

    private async void Submit_Goal_Clicked(object sender, EventArgs e)
    {
        
        try
        {
            Goal = Convert.ToInt32(Step_goal.Text);
            SetBar = true;
            Step_goal.Text = string.Empty;            
            steps = 0;

        }
        catch(Exception ex) 
        {
            await DisplayAlert("Chyba", $"{ex}", "Ok");
        }
        
        
    }

}