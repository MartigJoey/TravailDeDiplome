﻿#pragma checksum "..\..\..\PageSimulation.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1C12738F48BD4B6ECB4E50A69E5C256FAD8DE709"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using CovidPropagation;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
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


namespace CovidPropagation {
    
    
    /// <summary>
    /// PageSimulation
    /// </summary>
    public partial class PageSimulation : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 36 "..\..\..\PageSimulation.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider intervalSlider;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\PageSimulation.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblDate;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\..\PageSimulation.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnStart;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\..\PageSimulation.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnBreak;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\..\PageSimulation.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnReset;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\..\PageSimulation.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnOpenRawDatas;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\..\PageSimulation.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer slvScroller;
        
        #line default
        #line hidden
        
        
        #line 85 "..\..\..\PageSimulation.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grdContent;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.5.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/CovidPropagation;component/pagesimulation.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\PageSimulation.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.5.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.intervalSlider = ((System.Windows.Controls.Slider)(target));
            
            #line 40 "..\..\..\PageSimulation.xaml"
            this.intervalSlider.AddHandler(System.Windows.Controls.Primitives.Thumb.DragCompletedEvent, new System.Windows.Controls.Primitives.DragCompletedEventHandler(this.IntervalSlider_DragCompleted));
            
            #line default
            #line hidden
            return;
            case 2:
            this.lblDate = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.btnStart = ((System.Windows.Controls.Button)(target));
            
            #line 54 "..\..\..\PageSimulation.xaml"
            this.btnStart.Click += new System.Windows.RoutedEventHandler(this.Start_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnBreak = ((System.Windows.Controls.Button)(target));
            
            #line 59 "..\..\..\PageSimulation.xaml"
            this.btnBreak.Click += new System.Windows.RoutedEventHandler(this.Break_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnReset = ((System.Windows.Controls.Button)(target));
            
            #line 65 "..\..\..\PageSimulation.xaml"
            this.btnReset.Click += new System.Windows.RoutedEventHandler(this.Reset_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnOpenRawDatas = ((System.Windows.Controls.Button)(target));
            
            #line 71 "..\..\..\PageSimulation.xaml"
            this.btnOpenRawDatas.Click += new System.Windows.RoutedEventHandler(this.OpenRawDatasWindow_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.slvScroller = ((System.Windows.Controls.ScrollViewer)(target));
            return;
            case 8:
            this.grdContent = ((System.Windows.Controls.Grid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

