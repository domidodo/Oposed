### Run example/demo

1. Clone the repo
   ```sh
   git clone https://github.com/Kinderschutzbund-Karlsruhe/Oposed.git
   ```
2. Jump into the `example`-directory
   ```sh
   cd example/
   ```
3. Make the script runnable
   ```sh
   chmod +x start.sh
   ```
4. Run `start.sh`
   ```sh
   ./start.sh
   ```
5. Open your browser on `http://localhost:8080` and log in with one of the following access data:

| Role  | Mail            | Username     | Password     |
| ----- | --------------- | ------------ | ------------ |
| Admin | admin@oposed.de | Oposed-Admin | Oposed-Admin |
| User  | user1@oposed.de | oposed-user1 | oposed-user1 |
| User  | user2@oposed.de | oposed-user2 | oposed-user2 |

You can manage the users over der <a href="https://github.com/wheelybird/ldap-user-manager">LDAP-User-Manager</a> under `http://localhost:8081/log_in/`
