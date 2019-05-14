echo $(pwd)

CheckoutProjects()
{
  #Checkout projects if they don't exist already
  if [ -d "$SNLA-app" ]; then
    echo "SNLA-app Already exist"
  else
    git clone git@github.com:snla-system/SNLA-app.git
  fi
}

CheckoutProjects
