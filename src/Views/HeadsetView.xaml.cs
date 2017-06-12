using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Genesyslab.Desktop.Infrastructure.DependencyInjection;
using Genesyslab.Desktop.Modules.Windows.Common.DimSize;
using Genesyslab.Enterprise.Model.ServiceModel;
using JabraInteractionExtension.Models;

namespace JabraInteractionExtension.Views
{
	/// <summary>
	/// Interaction logic for MySampleView.xaml
	/// </summary>
	public partial class HeadsetView : UserControl, IHeadsetView
	{
		static public IObjectContainer Container;
	  static public IEnterpriseServiceProvider Esp;

	  public HeadsetView(IPresentationModel presentationModel, IObjectContainer container, IEnterpriseServiceProvider esp)
	  {
	    this.Model = presentationModel;
	    Container = container;
	    Esp = esp;

	    InitializeComponent();

	    Width = Double.NaN;
	    Height = Double.NaN;
	    MinSize = new MSize() { Width = 200.0, Height = 200.0 };
	  }

	  MSize _MinSize;
		public MSize MinSize
		{
			get { return _MinSize; }  // (MSize)base.GetValue(MinSizeProperty); }
			set
			{
				_MinSize = value; // base.SetValue(MinSizeProperty, value);
				OnPropertyChanged("MinSize");
			}
		}


		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string name)
		{
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

		#endregion

		#region IMySampleView Members

		/// <summary>
		/// Gets or sets the model.
		/// </summary>
		/// <value>The model.</value>
		public IPresentationModel Model
		{
			get { return this.DataContext as IPresentationModel; }
			set { this.DataContext = value; }
		}

		#endregion

		#region IView Members

		/// <summary>
		/// Gets or sets the context.
		/// </summary>
		/// <value>The context.</value>
		public object Context { get; set; }

		/// <summary>
		/// Creates this instance.
		/// </summary>
		public void Create()
		{
		  //MessageBox.Show("Create");
		}

		/// <summary>
		/// Destroys this instance.
		/// </summary>
		public void Destroy()
		{
		  //MessageBox.Show("Destroy");
		}

    #endregion

    private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			Model.CustomAction1();
		}
	}
}
