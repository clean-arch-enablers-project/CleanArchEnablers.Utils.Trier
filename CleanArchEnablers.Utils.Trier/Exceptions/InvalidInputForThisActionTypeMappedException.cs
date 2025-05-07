using Cae.Utils.MappedExceptions;

namespace CleanArchEnablers.Utils.Trier.Exceptions;

public class InvalidInputForThisActionTypeMappedException() : MappedException("Input Is Null.", "Input cannot be null for this action type.");