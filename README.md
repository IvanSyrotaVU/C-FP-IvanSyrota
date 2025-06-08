C# Scanner by Ivan Syrota

-------------------------------------------------------------------------------

Task 1
-Set up the initial structure of the project with two separate applications

-Implemented basic data transfer between Scanner and Master using NamedPipeClientStream and NamedPipeServerStream

Added .gitignore file to exclude unnecessary Visual Studio files (e.g., .vs/, bin/, obj/) to prevent permission errors during commits and repository pollution*

Task 2
-Refactored the Scanner to support multithreading

-Each .txt file is processed by a separate thread

-Results are collected through thread-safe queues and sent to named pipes

Task 3
-Added processor affinity support

-Each thread is assigned to a separate CPU core using the ProcessorAffinity property of ProcessThread

Added general profile for stable pipe connection if the Master is not yet running*

Task 4
-Improved the Master interface with a status label

-The label updates based on the current state: receiving data or done

-The number of received items is displayed after data collection is complete

-------------------------------------------------------------------------------

How It Works:
Master is started first and listens to pipeA and pipeB

Scanner is launched and connects to both pipes

Files are processed in parallel, and data is sent to the Master

Master receives and displays the results
