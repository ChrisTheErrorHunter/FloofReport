﻿#pragma checksum "..\..\..\..\PageReportGenerator\PageReportGenerator.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "535F4714DCB89D70BF2995C2A6266BA0B55A1315"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using FloofReport;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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


namespace FloofReport {
    
    
    /// <summary>
    /// PageReportGenerator
    /// </summary>
    public partial class PageReportGenerator : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 14 "..\..\..\..\PageReportGenerator\PageReportGenerator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbCage;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\..\PageReportGenerator\PageReportGenerator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbMonths;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\..\PageReportGenerator\PageReportGenerator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnGenerate;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\..\PageReportGenerator\PageReportGenerator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbDays;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\..\PageReportGenerator\PageReportGenerator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbYears;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\..\PageReportGenerator\PageReportGenerator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAddToList;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\..\PageReportGenerator\PageReportGenerator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnXEgsamineGenerate;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/FloofReport;V1.0.0.0;component/pagereportgenerator/pagereportgenerator.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\PageReportGenerator\PageReportGenerator.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.cmbCage = ((System.Windows.Controls.ComboBox)(target));
            
            #line 14 "..\..\..\..\PageReportGenerator\PageReportGenerator.xaml"
            this.cmbCage.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cmbCage_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.cmbMonths = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 3:
            this.btnGenerate = ((System.Windows.Controls.Button)(target));
            
            #line 16 "..\..\..\..\PageReportGenerator\PageReportGenerator.xaml"
            this.btnGenerate.Click += new System.Windows.RoutedEventHandler(this.btnGenerate_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.cmbDays = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 5:
            this.cmbYears = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 6:
            this.btnAddToList = ((System.Windows.Controls.Button)(target));
            
            #line 19 "..\..\..\..\PageReportGenerator\PageReportGenerator.xaml"
            this.btnAddToList.Click += new System.Windows.RoutedEventHandler(this.btnAddToList_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.btnXEgsamineGenerate = ((System.Windows.Controls.Button)(target));
            
            #line 26 "..\..\..\..\PageReportGenerator\PageReportGenerator.xaml"
            this.btnXEgsamineGenerate.Click += new System.Windows.RoutedEventHandler(this.btnGenerate_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

