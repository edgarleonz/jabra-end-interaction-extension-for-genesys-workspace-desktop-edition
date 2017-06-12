using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Genesyslab.Desktop.Infrastructure;
using Genesyslab.Desktop.Infrastructure.DependencyInjection;
using Genesyslab.Desktop.Modules.Core.Model.Interactions;
using Genesyslab.Desktop.Modules.Windows.Event;
using JabraInteractionExtension.Models;
using JabraInteractionExtension.Views;

namespace JabraInteractionExtension.Views
{
  /// <summary>
  /// Interaction logic for MainToolbarContainerView.xaml
  /// </summary>
  public partial class HeadsetButtonView : UserControl, IHeadsetButtonView
  {
    readonly IObjectContainer container;
    readonly IViewEventManager viewEventManager;

    private DispatcherTimer timer;

    public HeadsetButtonView(IPresentationModel presentationModel, IObjectContainer container,
      IViewEventManager viewEventManager)
    {
      this.container = container;
      this.viewEventManager = viewEventManager;
      this.Model = presentationModel;

      InitializeComponent();

      Width = Double.NaN;
      Height = Double.NaN;

      timer = new DispatcherTimer();
      timer.Interval = TimeSpan.FromSeconds(1);
      timer.Tick += timer_Tick;
      timer.Start();
    }

    private void timer_Tick(object sender, EventArgs e)
    {
      this.Model.IsInteractionVisible = IsVisible;
    }

    #region IView Members

    public object Context { get; set; }

    public void Create()
    {
      Model.Case = (Context as IDictionary<string, object>).TryGetValue("Case") as ICase;

      viewEventManager.Subscribe(ActionEventHandler);
    }

    public void Destroy()
    {
      viewEventManager.Unsubscribe(ActionEventHandler);

      Model.Case = null;
    }

    #endregion

    #region IMainToolbarContainerView Members

    public IPresentationModel Model
    {
      get { return this.DataContext as IPresentationModel; }
      set { this.DataContext = value; }
    }

    #endregion

    public void ActionEventHandler(object eventObject)
    {
      if (Application.Current.Dispatcher != null && !Application.Current.Dispatcher.CheckAccess())
        Application.Current.Dispatcher.Invoke(DispatcherPriority.Send, new Action<object>(ActionEventHandler),
          eventObject);
      else
      {
        GenericEvent contactEvent = eventObject as GenericEvent;

        if (contactEvent != null && contactEvent.Context == Model.Case.CaseId &&
            contactEvent.Target == GenericContainerView.ContainerView)
        {
          foreach (GenericAction contactAction in contactEvent.Action)
          {
            string objectSimpleAction = contactAction.Action as string;
            switch (objectSimpleAction)
            {
              // To use a 8.1.3.x plug-in with IW 8.1.3, use the following block
              // case ActionGenericContainerView.ShowHidePanelRight:
              //     splitToggleButton.IsChecked = ((Visibility)contactAction.Parameters[0] == Visibility.Visible && contactAction.Parameters[1] as string == "HeadsetInteraction");
              //     break;
              //
              // for use with IW 8.1.4+ to synchronize the side button with the visibility of the right panel 
              case ActionGenericContainerView.UserControlLoaded:
                splitToggleButton.IsChecked = ((Visibility) contactAction.Parameters[0] == Visibility.Visible &&
                                               contactAction.Parameters[1] as string == "HeadsetInteraction");
                break;
              default:
                break;
            }
          }
        }
      }
    }

    private void splitToggleButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      // Lock MinSize
      viewEventManager.Publish(new GenericEvent()
      {
        SourceId = null,
        Target = GenericContainerView.ContainerView,
        Context = Model.Case.CaseId,
        TargetId = null,
        Action = new GenericAction[]
        {
          new GenericAction()
          {
            Action = ActionGenericContainerView.LockMinSize,
            Parameters = new object[] {true, "InteractionContainerView"}
          }
        }
      });

      viewEventManager.Publish(new GenericEvent()
      {
        Target = GenericContainerView.ContainerView,
        Context = Model.Case.CaseId,
        Action = new GenericAction[]
        {
          new GenericAction()
          {
            Action = ActionGenericContainerView.ShowHidePanelRight,
            Parameters = new object[]
              {splitToggleButton.IsChecked ?? false ? Visibility.Visible : Visibility.Collapsed, "HeadsetInteraction"}
          },
          new GenericAction()
          {
            Action = ActionGenericContainerView.ActivateThisPanel,
            Parameters = new object[] {"HeadsetInteraction"}
          }
        }
      });

      // Unlock MinSize
      viewEventManager.Publish(new GenericEvent()
      {
        SourceId = null,
        Target = GenericContainerView.ContainerView,
        Context = Model.Case.CaseId,
        TargetId = null,
        Action = new GenericAction[]
        {
          new GenericAction()
          {
            Action = ActionGenericContainerView.LockMinSize,
            Parameters = new object[] {false, "InteractionContainerView"}
          }
        }
      });
    }
  }
}
