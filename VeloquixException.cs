using System;

namespace Veloquix.BotRunner.SDK;

public class VeloquixException(string message) : Exception($"Error In BotRunner: {message}");
