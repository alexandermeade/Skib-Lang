
![fire](https://github.com/AlexanderMeade/Skib-Lang/assets/128431625/d0e0aab8-c12c-47ac-97af-dd8439e9f9cb)

#adviosory note

I love lua this is pure parody 

# Skib-Lang
A superset of Lua that uses the most up-to-date syntax and coding conventions to both adhere to the newer generation's linguistic needs and not have to program in Lua.

The Skiblang compiler compiles directly to Lua and always embeds Lua and other stuff.

# Examples

SkibLang comes with a variety of features, such as functions.

As an example, this is a simple fizzbuzz program in Skiblang.

# comments and vars
Using the "rizz" keyword, we can define a local variable in lua terms to act as an iterator for our gyatt (loop), and once our iterator reaches a certain point, we can hit the breaks with caseoh.
```
 tiktokrizzparty this won't run lmao
 a = 3
 print(a)
```
# data tables

```
rizz a = [1,2,3,4]

rizz b = [name = "alex", age = 19]

print(b.name) tiktokrizzparty print datatable value: name
print(a) tiktokrizzparty prints table address

```


# fizzBuzz and functions!

tip: you can use the 'mrworldwide' keyword after the skibidi keyword to make the function public
```


tiktokrizzparty private version of fizzbuzz
 
skibidi fizzBuzz(n) {
    
    sus n%3==0 && n%5==0 lowtaperfade "fizzbuzz" 
    sus n%5==0 lowtaperfade "buzz"
    sus n%3==0 lowtaperfade "fizz"
    lowtaperfade n
}

skibidi privFizzBuzz(n) {
    
    sus n%3==0 && n%5==0 lowtaperfade "fizzbuzz" 
    sus n%5==0 lowtaperfade "buzz"
    sus n%3==0 lowtaperfade "fizz"
    lowtaperfade n
}

print(fizzBuzz(1))
print(fizzBuzz(3))
print(fizzBuzz(5))
print(fizzBuzz(15))



```


# anonymous functions
```

skibidi func(fn) fn()

func(
    skibidi _() print("hello :D ")
)

```

This program will then compile to Lua code: 
```

local function fizzBuzz(n )
			
if (((n) % (3)) == (0)) and (((n) % (5)) == (0)) then 
	
return "fizzbuzz"
end
	
if ((n) % (5)) == (0) then 
	
return "buzz"
end
	
if ((n) % (3)) == (0) then 
	
return "fizz"
end
	
return n	
end

print(
fizzBuzz(1)
)

print(
fizzBuzz(3)
)

print(
fizzBuzz(5)
)

print(
fizzBuzz(15)
)


```

# variable decleration, loops, breaking!


Now you can't have a modern programming language without loops, so let's put some in our Fizzbuzz program.
```

skibidi fizzBuzz(n) {
    
    sus n%3==0 && n%5==0 lowtaperfade "fizzbuzz" 
    sus n%5==0 lowtaperfade "buzz"
    sus n%3==0 lowtaperfade "fizz"
    lowtaperfade n
}

rizz a = 0

gyatt {
    sus a>=100 caseoh

    print(fizzBuzz(a))

    a = a + 1
}

```



As a note, the "pass" key word is for continue statements.

# embeding!

Now let's do some advanced skib scripting.
```

skibidi map(values, fn) {
    newValues = []

    ohio "
        for i, v in ipairs(values) do 
            newValues[#newValues+1] = fn(v)
        end
    "
    lowtaperfade newValues
}

skibidi fizzBuzz(n) {
    
    sus n%3==0 && n%5==0 lowtaperfade "fizzbuzz" 
    sus n%5==0 lowtaperfade "buzz"
    sus n%3==0 lowtaperfade "fizz"
    lowtaperfade n
}

map(map([1,2,3,4], fizzBuzz),skibidi _(v) print(v))

```

This program uses embedding via the "ohio" keyword to allow for the use of lua in skibLang, and with this, we can declare a generic function like map to run the fizzbuzz code below.

# pipes!

However, I'm not quite satisfied yet with the whole look of this, so let's use pipes to make it look good and to spite javascript. 

```

skibidi map(values, fn) {
    newValues = []

    ohio "
        for i, v in ipairs(values) do 
            newValues[#newValues+1] = fn(v)
        end
    "
    lowtaperfade newValues
}

skibidi fizzBuzz(n) {
    
    sus n%3==0 && n%5==0 lowtaperfade "fizzbuzz" 
    sus n%5==0 lowtaperfade "buzz"
    sus n%3==0 lowtaperfade "fizz"
    lowtaperfade n
}

[1,2,3,4,5] :3 map(fizzBuzz) :3 map(skibidi _(n) print(n))
```

Now, is that not the nicest code you've ever seen.

# modules

Now what about modules? I hear you cry well. You can use the keyword "bussin" to use your favorite modules!
```


bussin "math" math

math.pow(2,2) :3 print

```

# slotting

Now, with an important note, this will not import your files into Skiblang. Only the slot keyword can also be known as the "kaicenat" keyword.

=============<main.skib>=============

```
kaicenat "./otherFile.skib"

otherFile_print("hello world :D")
```

=============<otherFile.skib>=============

```
skibidi otherFile_print(word) print("otherFile: " ++ word)
```

With the kaicenat keyword, we can slot in the code into our main program!


# namespaceishes

Now the big filepath may be off-putting to you, and with good reason, but we can use the namespace keyword "livvy" to keep it short and sweet.

```
livvy "C:\\Users\\allon\\Desktop\\test"

kaicenat "otherFile.skib"

otherFile_print("hello world :D")
```





# uses

since everything in this langauge is built off of lua you can use lua libraries with it such as Love2D which I used here ^^


![image](https://github.com/AlexanderMeade/Skib-Lang/assets/128431625/d3ba7b83-abb1-4491-b1a8-3fc7f29595fc)


# Compiler Notes

# windows

to run the skib.cs compiler navigate to the publish directory and then you can freely run it using.

```
./skib.cs -r ./example.skib
```

# Linux

To run the skib.cs compiler you must first convert it into a runnable format. 


```
chmod +x ./skib.cs.pdb
```

after running this comand you can freely use the compiler

```
./skib.cs -r ./example.skib
```

if you want help with the compiler you can use the -h command!


# Extras

![image](https://github.com/alexandermeade/Skib-Lang/assets/128431625/3b470d26-7c97-4175-b5d4-5427092fe526)


Vim highlighting support with the skibHighligh plugin! https://github.com/alexandermeade/skibHighlight
