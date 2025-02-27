// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Shared.Application.Domain.Entities;

namespace a2p.Shared.Infrastructure.Interfaces
{
    public interface ILogService
    {

        // Verbose is used for detailed information.
        // It is used for debugging purposes.
        //===================================================================================================================================
        void Verbose(string message, params object[]? args);

        void Verbose(Exception ex, string message, params object[]? args);

        void Verbose(Exception ex);

        // Debug is used for internal system events that are not necessarily observable from the outside.
        // It is used for debugging purposes.
        //===================================================================================================================================
        void Debug(string message, params object[]? args);

        void Debug(Exception ex, string message, params object[]? args);

        void Debug(Exception ex);

        // Information is used for expected behavior.
        // It is used when the application is able to continue.
        //===================================================================================================================================
        void Information(string message, params object[]? args);

        void Information(Exception ex, string message, params object[]? args);

        void Information(Exception ex);

        // Warning is used for unexpected behavior that is not critical.
        // It is used when the application is able to continue.
        //===================================================================================================================================
        void Warning(string message, params object[]? args);

        void Warning(Exception ex, string message, params object[]? args);

        void Warning(Exception ex);

        // Error is used for unexpected behavior that should be investigated.
        // It is used when the application is able to continue.
        //===================================================================================================================================
        void Error(string message, params object[]? args);

        void Error(Exception ex, string message, params object[]? args);

        void Error(Exception ex);

        // Fatal is the highest level of severity.
        // It is used to indicate that the application is about to terminate.
        //===================================================================================================================================
        void Fatal(Exception ex, string message, params object[]? args);

        void Fatal(string message, params object[]? args);

        void Fatal(Exception ex);

        // CloseAndFlush is used to close and flush the logger.
        //===================================================================================================================================
        void CloseAndFlush();

        void DeleteLogFiles();

        Task<List<A2PLogRecord>> GetRepository(string file);
    }

}

