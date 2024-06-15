VAR currentDialogue = ""

{currentDialogue == "morningGreeting":
        Aphrodi: colere : Me parle pas dès le matin
        Lou : surpris : Ok ça marche
        -> END
    - else :
        Aphrodi: joyeux : Salut c'est Aphrodi
        Lou : joyeux : Salut c'est Lou
        -> END
}