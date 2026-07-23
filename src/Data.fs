// HITO 6 — Catálogo del prototipo y datos del negocio
module DonPizzero.Data

open DonPizzero.Types

// ═══ Negocio ═══
let negocio = {
    Nombre = "Don Pizzero"
    Ciudad = "Ayacucho"
    WhatsApp = "51990904900"      // número general de pedidos para todos los locales
    Horarios = [
        { Dias = "Lun - Dom"; Apertura = "17:00"; Cierre = "23:00" }   // TODO: real
    ]
    DeliveryGratis = true
    Facebook = "https://www.facebook.com/DonPizzeroAyacucho"
    TikTok = "https://www.tiktok.com/@donpizzeroayacucho"
    Locales = [
        { Nombre = "Jr. Asamblea";         Maps = "https://maps.app.goo.gl/aKNXfMQsUsUPVyAHA" }
        { Nombre = "Av. San Lorenzo";      Maps = "https://maps.app.goo.gl/tpK9dvh7MiShZkp3A" }
        { Nombre = "Jr. Libertad";         Maps = "https://maps.app.goo.gl/cJHTraCiTuvT7Nyy5" }
        { Nombre = "Jr. Choro";            Maps = "https://maps.app.goo.gl/qWivSzViRYtPapyFA" }
        { Nombre = "Jr. Sur";              Maps = "https://maps.app.goo.gl/KsQurCWVzeqsVtD57" }
        { Nombre = "Jr. Los Morochucos";   Maps = "https://maps.app.goo.gl/EZJSJhBYsixYpfn7A" }
        { Nombre = "Av. Pérez de Cuéllar"; Maps = "https://maps.app.goo.gl/aZ9ntcYEmq2wJ52D6" }
    ]
}

// ═══ Extras (solo pizzas) ═══
let extrasPizza = [
    { Id = "ex-queso";    Nombre = "Queso extra";    Precio = 5.0m }
    { Id = "ex-borde";    Nombre = "Borde de queso"; Precio = 6.0m }
    { Id = "ex-chorizo";  Nombre = "Chorizo";        Precio = 4.0m }
    { Id = "ex-champi";   Nombre = "Champiñones";    Precio = 4.0m }
    { Id = "ex-pina";     Nombre = "Piña";           Precio = 3.0m }
    { Id = "ex-aceituna"; Nombre = "Aceitunas";      Precio = 3.0m }
]

let private tresTamanos personal mediana familiar =
    PorTamano (Map.ofList [
        Personal, personal
        Mediana, mediana
        Familiar, familiar
    ])

// ═══ Catálogo (precios "desde" = tamaño personal, según prototipo) ═══
let menu : MenuItem list = [
    // ── Clásicas ──
    { Id = "pz-americana"
      Nombre = "Pizza Americana"
      Descripcion = "Pepperoni, jamón y tocino sobre queso derretido"
      Emoji = "🥓"
      Categoria = Clasicas
      Precio = tresTamanos 26.0m 36.0m 46.0m
      ExtrasDisponibles = extrasPizza
      Etiqueta = Some "Más pedida"
      Popular = true }

    { Id = "pz-hawaiana"
      Nombre = "Pizza Hawaiana"
      Descripcion = "Jamón, piña y mozzarella. Un clásico dulce-salado"
      Emoji = "🍍"
      Categoria = Clasicas
      Precio = tresTamanos 25.0m 35.0m 45.0m
      ExtrasDisponibles = extrasPizza
      Etiqueta = None
      Popular = true }

    { Id = "pz-pepperoni"
      Nombre = "Pizza Pepperoni"
      Descripcion = "Doble pepperoni y mozzarella. Simple y poderosa"
      Emoji = "🍕"
      Categoria = Clasicas
      Precio = tresTamanos 24.0m 34.0m 44.0m
      ExtrasDisponibles = extrasPizza
      Etiqueta = None
      Popular = false }

    { Id = "pz-margarita"
      Nombre = "Pizza Margarita"
      Descripcion = "Mozzarella, tomate y albahaca fresca"
      Emoji = "🧀"
      Categoria = Clasicas
      Precio = tresTamanos 22.0m 32.0m 42.0m
      ExtrasDisponibles = extrasPizza
      Etiqueta = None
      Popular = false }

    // ── Especiales ──
    { Id = "pz-mexicana"
      Nombre = "Pizza Mexicana"
      Descripcion = "Chorizo, jalapeño, pimiento y doble queso"
      Emoji = "🌮"
      Categoria = Especiales
      Precio = tresTamanos 27.0m 38.0m 49.0m
      ExtrasDisponibles = extrasPizza
      Etiqueta = Some "🔥 Picante"
      Popular = true }

    { Id = "pz-especial"
      Nombre = "Don Pizzero Especial"
      Descripcion = "Jamón, chorizo, tocino y pimiento — la receta de la casa"
      Emoji = "👑"
      Categoria = Especiales
      Precio = tresTamanos 29.0m 40.0m 52.0m
      ExtrasDisponibles = extrasPizza
      Etiqueta = Some "La de la casa"
      Popular = true }

    // ── Criollas ──
    { Id = "pz-criolla"
      Nombre = "Pizza Criolla"
      Descripcion = "Chorizo ayacuchano, cebolla criolla y ají amarillo"
      Emoji = "🍖"
      Categoria = Criollas
      Precio = tresTamanos 26.0m 37.0m 48.0m
      ExtrasDisponibles = extrasPizza
      Etiqueta = None
      Popular = false }

    { Id = "pz-huancaina"
      Nombre = "Pizza a la Huancaína"
      Descripcion = "Salsa huancaína, papa dorada y queso serrano"
      Emoji = "🥔"
      Categoria = Criollas
      Precio = tresTamanos 27.0m 38.0m 49.0m
      ExtrasDisponibles = extrasPizza
      Etiqueta = Some "Novedad"
      Popular = false }

    // ── Pastas ──
    { Id = "pa-lasana"
      Nombre = "Lasaña de Carne"
      Descripcion = "Capas de pasta con carne, bechamel y queso gratinado"
      Emoji = "🍝"
      Categoria = Pastas
      Precio = Fijo 18.0m
      ExtrasDisponibles = []
      Etiqueta = None
      Popular = false }

    { Id = "pa-fetuccini"
      Nombre = "Fetuccini a lo Alfredo"
      Descripcion = "Salsa cremosa, parmesano y pan al ajo"
      Emoji = "🥣"
      Categoria = Pastas
      Precio = Fijo 15.0m
      ExtrasDisponibles = []
      Etiqueta = None
      Popular = false }

    // ── Promociones ──
    { Id = "pr-duo"
      Nombre = "Promo Dúo Familiar"
      Descripcion = "2 pizzas familiares clásicas a elección"
      Emoji = "🎉"
      Categoria = Promociones
      Precio = Fijo 79.0m
      ExtrasDisponibles = []
      Etiqueta = Some "Ahorra S/ 13"
      Popular = false }

    { Id = "pr-combo"
      Nombre = "Combo Familiar"
      Descripcion = "Pizza familiar clásica + fetuccini + gaseosa 1.5L"
      Emoji = "🥤"
      Categoria = Promociones
      Precio = Fijo 59.0m
      ExtrasDisponibles = []
      Etiqueta = Some "Promo"
      Popular = false }
]
