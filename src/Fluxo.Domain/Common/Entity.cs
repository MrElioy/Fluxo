


namespace Fluxo.Domain.Common;


public abstract class Entity
{
    public Guid Id { get; protected set; }

    protected Entity()
    {
        Id = Guid.NewGuid();
    }
}

public enum MovementType
{
    Income,
    Expense,
    Transfer,
    Other
}

public enum Currency
{
    ARS,
    USD,
    EUR,
    Other
}
public enum Status
{
    Active,
    Inactive,
    Eliminated
}

//################################################################################################################
//FinancialMovement :  Representa un hecho económico que afecta una cuenta o la posición financiera del usuario.
//################################################################################################################
public class FinancialMovement : Entity
{
    public Guid AccountId { get; private set; } // Id de la cuenta a la que pertenece el movimiento financiero
    public DateTime TransactionDate { get; private set; }// Fecha de la transacción del movimiento financiero, se asigna automáticamente al momento de crear el movimiento
    public DateTime RegisteredAt { get; private set; } = DateTime.UtcNow; // Fecha de registro del movimiento financiero, se asigna automáticamente al momento de crear el movimiento
    public decimal Amount { get; private set; }// Monto del movimiento financiero, no debe ser negativo, se debe usar el campo MovementType para indicar si es un ingreso o un egreso
    public Currency Currency { get; private set; } = Currency.Other;// Moneda del movimiento financiero (ARS, USD, EUR, etc.)
    public MovementType MovementType { get; private set; } = MovementType.Other; // Tipo de movimiento financiero (Ingreso, Egreso, Transferencia, etc.)
    public string? Description { get; private set; }// Descripción del movimiento financiero
    public Status Status { get; private set; } = Status.Active; // Status del movimiento financiero
    public Guid? RelatedMovementId { get; private set; } // Id del movimiento relacionado
    public DateTime? UpdatedAt { get; private set; }// Fecha de última actualización del movimiento

    public FinancialMovement(Guid accountId, decimal amount, DateTime date, string description, MovementType movementType, Currency currency, Guid? relatedMovementId = null)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "El monto del movimiento financiero no puede ser negativo.");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("La descripción del movimiento financiero no puede estar vacía.", nameof(description));
        }

        if (date > DateTime.UtcNow)
        {
            throw new ArgumentOutOfRangeException(nameof(date), "La fecha del movimiento financiero no puede ser futura.");
        }

        AccountId = accountId;
        Amount = amount;
        TransactionDate = date;
        Description = description;
        MovementType = movementType;
        Currency = currency;
        RelatedMovementId = relatedMovementId;
    }
}

//################################################################################################################
//Category :  Representa una categoría para clasificar los movimientos financieros.
//################################################################################################################
public class Category : Entity
{
    public string Name { get; private set; }// Nombre de la categoría
    public string? Description { get; private set; }// Descripción de la categoría
    public bool IsActive { get; private set; } = true;// Indica si la categoría está activa o no
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;// Fecha de creación de la categoría
    public Guid? ParentCategoryId { get; private set; } // Id de la categoría padre, si es que existe

    public Category(string name)
    {
        Name = name;
    }
}


//################################################################################################################
//MovementCategory :  Representa la relación entre un movimiento financiero y una categoría.
//################################################################################################################
public class MovementCategory 
{
    public Guid MovementId { get; private set; }// Id del movimiento financiero
    public Guid CategoryId { get; private set; }// Id de la categoría

    public MovementCategory(Guid movementId, Guid categoryId)
    {
        MovementId = movementId;
        CategoryId = categoryId;
    }
}



