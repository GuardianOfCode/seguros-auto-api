using System.ComponentModel.DataAnnotations;

namespace SegurosAuto.Api.Dtos;

/// <summary>Datos necesarios para registrar un cliente nuevo.</summary>
public class CrearClienteRequest
{
    /// <summary>Nombre completo del cliente.</summary>
    /// <example>Carlos Mendoza</example>
    [Required, StringLength(150)]
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Documento de identidad (DNI / RUC / cédula). No puede repetirse.</summary>
    /// <example>001-200380-0003C</example>
    [Required, StringLength(30)]
    public string Identificacion { get; set; } = string.Empty;

    /// <summary>Correo electrónico válido.</summary>
    /// <example>carlos.mendoza@correo.com</example>
    [Required, EmailAddress, StringLength(150)]
    public string Correo { get; set; } = string.Empty;
}

/// <summary>Datos para actualizar un cliente existente (mismos campos y validaciones que al crear).</summary>
public class ActualizarClienteRequest : CrearClienteRequest { }

/// <summary>Cliente devuelto por la API.</summary>
public class ClienteResponse
{
    /// <summary>Identificador del cliente.</summary>
    /// <example>1</example>
    public int Id { get; set; }

    /// <summary>Nombre completo.</summary>
    /// <example>Juan Pérez</example>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Documento de identidad.</summary>
    /// <example>001-010190-0001A</example>
    public string Identificacion { get; set; } = string.Empty;

    /// <summary>Correo electrónico.</summary>
    /// <example>juan.perez@correo.com</example>
    public string Correo { get; set; } = string.Empty;
}