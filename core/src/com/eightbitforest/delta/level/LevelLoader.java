package com.eightbitforest.delta.level;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.files.FileHandle;
import com.badlogic.gdx.utils.JsonReader;
import com.badlogic.gdx.utils.JsonValue;
import com.eightbitforest.delta.objects.Exit;
import com.eightbitforest.delta.objects.Fuel;
import com.eightbitforest.delta.objects.Player;
import com.eightbitforest.delta.objects.base.GameObject;
import com.eightbitforest.delta.objects.walls.*;
import com.eightbitforest.delta.utils.Constants;

import java.util.ArrayList;

public class LevelLoader {
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


        // As soon as we hit an empty line or {, switch to properties
        for (int y = 0; y < lines.length; y++) {
            lines[y] = lines[y].replace("\r", "");
            lines[y] = lines[y].replace("\n", "");
            if (readingProperties) {
                if (!lines[y].isEmpty() && !lines[y].startsWith("#"))
                    propLines.add(lines[y]);
            } else {
                if (lines[y].isEmpty() || lines[y].startsWith("{"))
                    readingProperties = true;
                else
                    levelLines.add(lines[y]);
            }
        }

        JsonValue[][] properties = new JsonValue[levelLines.get(0).length()][levelLines.size()];
        for (int i = 0; i < propLines.size(); i++) {
            JsonValue json = new JsonReader().parse(propLines.get(i));
            if (json.has("x") && json.has("y")) {
                properties[json.getInt("x") - 1][json.getInt("y") - 1] = json;
//                ArrayList<GameObjectPolygon> objects = level.getObjectsAt(
//                        json.getInt("x") - 1,
//                        levelLines.size() - json.getInt("y"));
//                if (objects.size() == 1) {
//                    objects.get(0).setProperties(json);
//                } else {
//                    System.out.println("No object at property position");
//                }
//            } else {
//                System.out.println("Level property missing x and y");
//            }
        }

//        for (ArrayList<ArrayList<GameObjectPolygon>> col : level) {
//            for (ArrayList<GameObjectPolygon> objects : col) {
//                for (GameObjectPolygon object : objects) {
//                    object.onUpdate();
//                    object.setZIndex(object.getZ());
//                }
//            }
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
        GameObject gameObject = null;
        switch (c) {
            case '▲':
                gameObject = new WallTriangleUp(level, x, y * Constants.TRIANGLE_HEIGHT);
                break;
            case '▼':
                gameObject = new WallTriangleDown(level, x, y * Constants.TRIANGLE_HEIGHT);
                break;
            case '◢':
                gameObject = new WallTriangleHalfTopLeft(level, x, y * Constants.TRIANGLE_HEIGHT);
                break;
            case '◤':
                gameObject = new WallTriangleHalfBottomRight(level, x, y * Constants.TRIANGLE_HEIGHT);
                break;
            case '◥':
                gameObject = new WallTriangleHalfBottomLeft(level, x, y * Constants.TRIANGLE_HEIGHT);
                break;
            case '◣':
                gameObject = new WallTriangleHalfTopRight(level, x, y * Constants.TRIANGLE_HEIGHT);
                break;
            case '◿':
                gameObject = new WallTriangleHalfTopLeftLong(level, x, y * Constants.TRIANGLE_HEIGHT);
                break;
            case '◸':
                gameObject = new WallTriangleHalfBottomRightLong(level, x, y * Constants.TRIANGLE_HEIGHT);
                break;
            case '◹':
                gameObject = new WallTriangleHalfBottomLeftLong(level, x, y * Constants.TRIANGLE_HEIGHT);
                break;
            case '◺':
                gameObject = new WallTriangleHalfTopRightLong(level, x, y * Constants.TRIANGLE_HEIGHT);
                break;
            case '^':
                gameObject = new WallTriangleInvertedUp(level, x, y * Constants.TRIANGLE_HEIGHT);
                break;
            case '■':
                gameObject = new WallBox(level, x, y * Constants.TRIANGLE_HEIGHT);
                break;
            case 'E':
                gameObject = new Exit(level, x, y * Constants.TRIANGLE_HEIGHT);
                break;
            case 'F':
                gameObject = new Fuel(level, x, y * Constants.TRIANGLE_HEIGHT);
                break;
            case 'P':
                gameObject = new Player(level, x, y * Constants.TRIANGLE_HEIGHT);
                level.setPlayer((Player) gameObject);
                return gameObject;
            default:
                if (c != ' ')
                    Gdx.app.error(Constants.TAG, "Cannot find gameobject with symbol " + c);
                return null;
        }
        level.addObject(gameObject);
        return gameObject;
    }
}
