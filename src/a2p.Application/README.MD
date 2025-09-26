# Application Layer

-  Use Interfaces for behavior (what you can do).
-  Use Contracts for data (what you send/receive).
---
- Interfaces → implemented by Infrastructure
- Contracts → used by Presentation & Application for requests/responses/events
```
Application/
  Interfaces/   <-- IOrderRepository, IExcelReader...
  Contracts/    <-- DTOs, Commands, Queries, Events
  Services/     <-- Use cases orchestrating everything
  ```