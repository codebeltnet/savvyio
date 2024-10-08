﻿using System;
using System.Threading.Tasks;

namespace Savvyio
{
    /// <summary>  
    /// Represents the method that will handle an event when the event provides data asynchronously.  
    /// </summary>  
    /// <typeparam name="TEventArgs">The type of the event data generated by the event.</typeparam>  
    /// <param name="sender">The source of the event.</param>  
    /// <param name="e">An object that contains the event data.</param>  
    /// <returns>A task that represents the asynchronous operation.</returns>  
    public delegate Task AsyncEventHandler<in TEventArgs>(object sender, TEventArgs e) where TEventArgs : EventArgs;
}
