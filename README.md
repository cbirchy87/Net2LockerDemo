# Net2LockerDemo

This is a simple non production ready example of how to use Net2 IO boards to control lockers.

Setup
Net2
Make sure your licence file is placed in the correct folder and the Net2 services/or PC is restarted before running this app
1. Make sure there is a Net2 Plus.This is the main controller
2. Create a users for each locker. Name these user after the lockers, for example Locker 1, Locker 2 etc
3. Creatre Trigger and action for EACH of these users. When a user is gratned access through a door > YOUR USER > effect a reply > Open relay of IO Board

Demo APP
Fill in details in the Servers>Net2CommsService.cs


When running the page will find the users. Pressing on the button will be an example of your code. A new pin is generated. This pin will then allow access to that locker. 
