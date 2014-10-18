﻿using System;

namespace GarageController
{
    public interface ILog
    {
        void Debug(string message);
        void Debug(string message, Exception ex);
        void Error(string message);
        void Error(string message, Exception ex);
        void Info(string message);
        void Info(string message, Exception ex);
        void Warn(string message);
        void Warn(string message, Exception ex);
    }
}
