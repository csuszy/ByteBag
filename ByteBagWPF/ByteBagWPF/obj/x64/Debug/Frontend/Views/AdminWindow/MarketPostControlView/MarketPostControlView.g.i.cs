﻿#pragma checksum "..\..\..\..\..\..\..\Frontend\Views\AdminWindow\MarketPostControlView\MarketPostControlView.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "5FF5114E8FFCAB6288606248DD78F649FEF5D9254465EB73A73D9350735AAE72"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ByteBagWPF.Frontend.Views.AdminWindow.MarketPostControlView;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace ByteBagWPF.Frontend.Views.AdminWindow.MarketPostControlView {
    
    
    /// <summary>
    /// MarketPostControlView
    /// </summary>
    public partial class MarketPostControlView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 45 "..\..\..\..\..\..\..\Frontend\Views\AdminWindow\MarketPostControlView\MarketPostControlView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel PostInfo;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\..\..\..\..\Frontend\Views\AdminWindow\MarketPostControlView\MarketPostControlView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid PostInfoGrid;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\..\..\..\..\Frontend\Views\AdminWindow\MarketPostControlView\MarketPostControlView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox postListLB;
        
        #line default
        #line hidden
        
        
        #line 103 "..\..\..\..\..\..\..\Frontend\Views\AdminWindow\MarketPostControlView\MarketPostControlView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RefreshBT;
        
        #line default
        #line hidden
        
        
        #line 128 "..\..\..\..\..\..\..\Frontend\Views\AdminWindow\MarketPostControlView\MarketPostControlView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button DeleteBT;
        
        #line default
        #line hidden
        
        
        #line 152 "..\..\..\..\..\..\..\Frontend\Views\AdminWindow\MarketPostControlView\MarketPostControlView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button editBT;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ByteBagWPF;component/frontend/views/adminwindow/marketpostcontrolview/marketpost" +
                    "controlview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\..\..\Frontend\Views\AdminWindow\MarketPostControlView\MarketPostControlView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.PostInfo = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 2:
            this.PostInfoGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.postListLB = ((System.Windows.Controls.ListBox)(target));
            
            #line 52 "..\..\..\..\..\..\..\Frontend\Views\AdminWindow\MarketPostControlView\MarketPostControlView.xaml"
            this.postListLB.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.postListLB_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.RefreshBT = ((System.Windows.Controls.Button)(target));
            
            #line 104 "..\..\..\..\..\..\..\Frontend\Views\AdminWindow\MarketPostControlView\MarketPostControlView.xaml"
            this.RefreshBT.Click += new System.Windows.RoutedEventHandler(this.refreshBT_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.DeleteBT = ((System.Windows.Controls.Button)(target));
            
            #line 129 "..\..\..\..\..\..\..\Frontend\Views\AdminWindow\MarketPostControlView\MarketPostControlView.xaml"
            this.DeleteBT.Click += new System.Windows.RoutedEventHandler(this.DeleteBT_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.editBT = ((System.Windows.Controls.Button)(target));
            
            #line 153 "..\..\..\..\..\..\..\Frontend\Views\AdminWindow\MarketPostControlView\MarketPostControlView.xaml"
            this.editBT.Click += new System.Windows.RoutedEventHandler(this.editBT_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

