using System;

namespace Bsg.Launcher.Updating;

public delegate void UpdateInstallationProblemEventHandler(Exception error, ref bool retry);
