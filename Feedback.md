### Feedback

*Please add below any feedback you want to send to the team*

- Regarding the CancellationToken, I'm using it in all the Commands even though it is not recommended to use it
in requests that change the state of the application because it might lead to data integrity issues.
I'm using it here because it is only a test and, as I understand, I should demonstrate how I use it.

- Regarding the Logging functionality, I'm using the default build in logging system from Asp.Net Core. In professional
environments I prefere structured logging with tools like Serilog. For the sake of time Im using the default Asp.Net core logging system.

- Regarding Redis, I would normally set up a url configuration in appsettings.json. But again, for the sake of time I hardcoded the host
and port numbers in the ConnectionHelper class.
