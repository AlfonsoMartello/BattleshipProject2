# Meeting notes:

**Meeting 1:**

Date: Fri 9/30/19

Location: Eaton 2

Members: Karen Setiawan, Rikki Augustine, Antonette Gichohu, Alfonso Martello, Grant Gollier

Topics:
- Features approved by Dr. Gibbons:
    - Ships snap onto grid when placed
    - A high score board. The scoreboard must retain its data between games
    - The required AI
- Bug Fixes:
    - Quit function
    - During ship placement, player 1 can place their ships and player 2's ships

**Meeting 1.5:**

Date: Fri 9/30/19

Location: Engineering Atrium

Members: Rikki Augustine, Alfonso Martello, Grant Gollier

Topics:
- Getting started with understanding Unity
- Dividing up work and features

**Meeting 2:**

Date: Wed 10/2/19

Location: Eaton 2

Members: Rikki Augustine, Alfonso Martello, Grant Gollier

Topics:
- Looked at documentation

**Meeting 3:**

Date: Mon 10/7/19

Location: Eaton 2

Members: Karen Setiawan, Antonette Gichohu, Alfonso Martello, Grant Gollier

Topics:
- Fall break schedule conflicts discussed
- Grant: Work on snap to grid functionality
- Alfonso: Starting AI integration
- Others: Getting up to speed with Unity

**Meeting 4:**

Date: Wed 10/9/19

Location: Spahr Library

Members: Karen Setiawan, Rikki Augustine, Antonette Gichohu, Alfonso Martello, Grant Gollier

Topics:
- Change the UI to give user the option of playing against a player or an AI
- Currently working on:
    - Snap to grid
    - AI
    - UI components
    - Not crying

**Meeting 5:**

Date: Mon 10/14

Location: Spahr Library

Members: Karen Setiawan, Rikki Augustine, Antonette Gichohu, Alfonso Martello, Grant Gollier

Topics:
- Figured out drop down and main menu UI
- Familiarized ourselves with unity
- **Watched Alfonso eat a 16-inch sandwich**
- Completed the scoreboard feature
- Completed the snap to grid feature
- Made a lot of changes to the UI

**Meeting 6:**

Date: Tue 10/15/2019

Location: Spahr Library

Members: Karen Setiawan, Rikki Augustine, Antonette Gichohu, Alfonso Martello, Grant Gollier

Topics:
- Worked on AI
    - Finished easy and hard AI
- Finished all of the changes to UI
- On AI turn, the user still needs to press the “fire” button
- Normal AI code still needs to be done

**Meeting 7:**

Date: Wed 10/16/2019

Location: Spahr Library

Members: Karen Setiawan, Rikki Augustine, Antonette Gichohu, Alfonso Martello, Grant Gollier

Topics:
- Documentation
- Normal AI Progress

**Meeting 8:**

Date: Thurs 10/18/2019

Location: Engineering

Members: Antonette Gichohu, Alfonso Martello, Grant Gollier, Rikki Augustine

Topics:
- How to wrapup normal AI
- Start work on Retrospective
- Wrapup documentation

# Team Division of Labor:
  - General help with AI and UI elements – Alfonso
  - Scoreboard UI/Easy AI – Karen
  - Snap to grid and scoreboard backend – Grant
  - Hard AI – Antonette
  - Normal AI - Rikki
  
# Challenges:
- Saving Unity UI elements: When we first started working, we did not know that we had to save the Unity project in addition to the changed code scripts when updating Unity's UI. This led to a few hours of trying to figure out why code being pulled from GitHub did not have the UI changes that team members were making.
- Learning how to use Unity: Learning how to use Unity was especially difficult because most tutorials deal with creating a new project from scratch, not working with an already existing project. This was overcome by spending large amounts of time doing nothing but trying to teach ourselves the basics of Unity so we could start writing code.
- Understanding previous team's documentation: The team we inherited from was ahead of the curve, as in they generated their code documentation using UML standards before we learned UML standards in class, so that was confusing at first. This problem was overcome because we eventually did learn UML in class.
- Not having an array backed grid for the player boards: The previous team did not use a data structure to store information about the players' boards, instead they used the image of an individual grid square to check whether or not a place had already been shot at or if that place was selected by the user. Thus, creating an AI that had knowledge of the board (hard and normal AI) was especially difficult because we had to use the image of individual grid squares to make AI decisions.
- UI set up: All of the Unity UI is contained in one scene, and what the player(s) see on the screen is controlled by setting everything else in the scene to invisible. This was a challenge because when we wanted to add new UI elements we had to move all of the other UI elements out of the way in the Unity editor so we could see what we were working on.

# Features not in the Demo:
- Having the AI turn happen without the user having to press the fire button.
- Having an impossible AI mode that would just end the game with a "you lose" message if the user selected it.

# Retrospective
- If we had more time we probably would have reworked the background logic for the game flow so that the players' boards were array-backed. We also would have split the Unity UI into multiple scenes instead of having a bunch of UI elements stacked on top of each other. We ended up completing most of the project over two days during Fall Break, but we could have started doing more coding and work earlier. We could have also found a more efficient way to split the work into smaller chunks so some people did not get shouldered with the more difficult tasks alone. Overall however, we worked really well as a team and our meetings were highly productive.
