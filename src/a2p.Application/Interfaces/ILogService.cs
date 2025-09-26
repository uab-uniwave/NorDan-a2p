using a2p.Application.DTO;

namespace a2p.Application.Interfaces
{
    public interface ILogService
    {


        // Verbose is used for detailed information.
        // It is used for debugging purposes.
        //===================================================================================================================================
        void Verbose(string message, params object[]? args);

       // Debug is used for internal system events that are not necessarily observable from the outside.
        // It is used for debugging purposes.
        //===================================================================================================================================
        void Debug(string message, params object[]? args);

        // Information is used for expected behavior.
        // It is used when the application is able to continue.
        //===================================================================================================================================
        void Information(string message, params object[]? args);

        // Warning is used for unexpected behavior that is not critical.
        // It is used when the application is able to continue.
        //===================================================================================================================================
        void Warning(string message, params object[]? args);

        // Error is used for unexpected behavior that should be investigated.
        // It is used when the application is able to continue.
        //===================================================================================================================================
        void Error(string message, params object[]? args);

        // Fatal is the highest level of severity.
        // It is used to indicate that the application is about to terminate.
        //===================================================================================================================================
        void Fatal(Exception ex, string message, params object[]? args);

        // CloseAndFlush is used to close and flush the logger.
        //===================================================================================================================================
        void CloseAndFlush();

        void DeleteLogFiles();

        Task<List<A2PLogRecord>> GetRepository(string file);
    }

}

