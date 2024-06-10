VAR currentDialogue = ""

{currentDialogue == "morningGreeting":
        Aphrodi: Me parle pas dès le matin
        Lou : Ok ça marche
        -> END
    - else:
        Aphrodi: Salut c'est Aphrodi
        Lou : Salut c'est Lou
        -> END
}