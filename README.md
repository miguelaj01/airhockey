# Project 2 – 2v2 Air Hockey with Unity ML-Agents

## Overview

This project is a playable 2v2 Air Hockey game built in Unity using C# and Unity ML-Agents. A human player teams up with one trained ML-Agent to play against two other ML-Agent paddles.

The game includes a rectangular rink, walls, two goals, a puck, four paddles, scoring, role selection, board orientation options, and a restart system.

## Game Rules

- The game is played as a 2v2 air hockey match.
- Each team has one Defender and one Striker.
- Each goal is worth 1 point.
- The game is played to 7 points.
- After a goal is scored, the puck resets to the center.
- The team scored upon serves next.
- The puck can bounce off the walls and enter either goal.

## Player Roles

Before the match starts, the human player can choose:

- Play as Striker
- Play as Defender

If the human chooses Striker, the AI controls the Defender on the human team.  
If the human chooses Defender, the AI controls the Striker on the human team.

## Movement Zones

The paddles are restricted based on their roles:

- Defender: moves only in the back 25 percent of the board near the goal.
- Striker: moves from the 25 percent mark up to the center line.
- The right team uses mirrored movement zones.
- Strikers cannot cross the center line.

## UI Features

The game uses Unity UI Toolkit for the interface.

The UI includes:

- Play as Striker button
- Play as Defender button
- Vertical Board button
- Horizontal Board button
- Score display
- Current role display
- Restart Match button
- Winner message when a team reaches 7 points

## ML-Agents Training

This project uses two separate ML-Agent behaviors:

### Striker Agent

The Striker Agent is trained to intercept the puck and shoot toward the opponent's goal.

Observations:
- Agent position
- Puck position
- Puck velocity
- Relative distance from the agent to the puck

Actions:
- Continuous movement in the X direction
- Continuous movement in the Z direction

Rewards:
- Small penalty over time to encourage active play
- Reward for moving close to the puck
- Reward for touching the puck
- Reward for hitting the puck forward
- Large reward for scoring
- Penalty if the opponent scores

### Defender Agent

The Defender Agent is trained to block incoming shots and keep the puck away from its own goal.

Observations:
- Agent position
- Puck position
- Puck velocity
- Relative distance from the agent to the puck

Actions:
- Continuous movement in the X direction
- Continuous movement in the Z direction

Rewards:
- Small penalty over time to encourage active defending
- Reward for being close to the puck when it is in the defensive zone
- Reward for hitting the puck
- Reward for pushing the puck away from its own goal
- Penalty if the opponent scores

## Neural Network Models

After training, the Striker and Defender models are exported as ONNX files and placed into the Behavior Parameters of the AI paddles.

The Behavior Parameters use:

- Vector Observation Size: 8
- Continuous Actions: 2
- Behavior Type: Inference Only or Default
- Behavior Names:
  - StrikerBehavior
  - DefenderBehavior

## Controls

Human player controls:

- W / Up Arrow: move up
- S / Down Arrow: move down
- A / Left Arrow: move left
- D / Right Arrow: move right

## Project Deliverables

This submission includes:

- Demo video showing ML training
- Demo video showing gameplay
- Standalone player build
- Unity project files
- ML-Agent scripts
- Behavior Parameters screenshots
- Documentation describing training, observations, actions, and rewards
