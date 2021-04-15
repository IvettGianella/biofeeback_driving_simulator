using BiofeebackDrivingSimulator.ViewModels;

namespace BiofeebackDrivingSimulator.Infrastructure
{
    public class InstanceLocator
    {
        public MainViewModel Main { get; set; }

        public InstanceLocator()
        {
            this.Main = MainViewModel.GetInstance();
        }
    }
}
