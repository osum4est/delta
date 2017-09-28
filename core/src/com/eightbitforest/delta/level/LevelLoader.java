package com.eightbitforest.delta.level;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.files.FileHandle;
import com.badlogic.gdx.utils.JsonReader;
import com.badlogic.gdx.utils.JsonValue;
import com.eightbitforest.delta.objects.Enemy;
import com.eightbitforest.delta.objects.Exit;
import com.eightbitforest.delta.objects.Fuel;
import com.eightbitforest.delta.objects.Player;
import com.eightbitforest.delta.objects.base.GameObject;
import com.eightbitforest.delta.objects.walls.*;
import com.eightbitforest.delta.utils.Constants;

import java.lang.reflect.Constructor;
import java.util.ArrayList;
import java.util.HashMap;

public class LevelLoader {
    private static HashMap<Character, Class<? extends GameObject>> objectCharMap =
            new HashMap<Character, Class<? extends GameObject>>() {{
                put('P', Player.class);
                put('E', Exit.class);
                put('F', Fuel.class);
                put('B', Enemy.class);

                put('▲', WallTriangleUp.class);
                put('▼', WallTriangleDown.class);
                put('◢', WallTriangleHalfTopLeft.class);
                put('◥', WallTriangleHalfBottomLeft.class);
                put('◣', WallTriangleHalfTopRight.class);
                put('◤', WallTriangleHalfBottomRight.class);
                put('◿', WallTriangleHalfTopLeftLong.class);
                put('◹', WallTriangleHalfBottomLeftLong.class);
                put('◺', WallTriangleHalfTopRightLong.class);
                put('◸', WallTriangleHalfBottomRightLong.class);
                put('^', WallTriangleInvertedUp.class);
                put('■', WallBox.class);
            }};

    public static Level loadLocalLevel(String packname, String filename) {
        FileHandle handle = Gdx.files.internal("levels/" + packname + "/" + filename);
        String file = handle.readString();
        return loadFromString(file);
    }

    private static Level loadFromString(String levelString) {
        Level level = new Level();

        String[] lines = levelString.split("\n");
        ArrayList<String> levelLines = new ArrayList<String>();
        ArrayList<String> propLines = new ArrayList<String>();

        boolean readingProperties = false;
        int levelWidth = 0;
        int levelHeight = 0;

        // As soon as we hit a {, switch to properties
        for (int y = 0; y < lines.length; y++) {
            lines[y] = lines[y].replace("\r", "");
            lines[y] = lines[y].replace("\n", "");
            if (readingProperties) {
                if (!lines[y].isEmpty() && !lines[y].startsWith("#"))
                    propLines.add(lines[y]);
            } else {
                if (lines[y].startsWith("{") || lines[y].startsWith("#")) {
                    readingProperties = true;
                    y--; // Decrement y so that we can add the property that starts with {
                } else {
                    levelLines.add(lines[y]);
                    if (lines[y].length() > levelWidth)
                        levelWidth = lines[y].length();
                }
            }
        }
        levelHeight = levelLines.size();

        JsonValue[][] properties = new JsonValue[levelWidth][levelHeight];
        for (int i = 0; i < propLines.size(); i++) {
            JsonValue json = new JsonReader().parse(propLines.get(i));

            if (json.has("x") && json.has("y"))
                properties[json.getInt("x") - 1][json.getInt("y") - 1] = json;
        }

        for (int y = 0; y < levelLines.size(); y++) {
            for (int x = 0; x < levelLines.get(y).length(); x++) {
                char c = levelLines.get(y).charAt(x);
                GameObject object = addGameObject(level, c, x, levelLines.size() - 1 - y);
                if (properties[x][y] != null && object != null) {
                    object.setProperties(properties[x][y]);
                }
            }
        }

        return level;
    }

    private static GameObject addGameObject(Level level, char c, int x, int y) {
        GameObject gameObject;

        if (c == ' ')
            return null;

        if (!objectCharMap.containsKey(c)) {
            Gdx.app.error(Constants.TAG, "Cannot find gameobject with char " + c);
            return null;
        }

        Constructor<? extends GameObject> constructor;
        try {
            constructor = objectCharMap.get(c).getConstructor(Level.class, float.class, float.class);
            gameObject = constructor.newInstance(level, x, y * Constants.TRIANGLE_HEIGHT);
        } catch (Exception e) {
            Gdx.app.error(Constants.TAG, "Unable to invoke constructor for " + c);
            return null;
        }

        level.addObject(gameObject);
        return gameObject;
    }
}
